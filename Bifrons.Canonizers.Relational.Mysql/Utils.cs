﻿using Bifrons.Lenses.Relational.Model;
using MySql.Data.MySqlClient;

namespace Bifrons.Canonizers.Relational.Mysql;

internal static class Utils
{

    internal static Result<TData> WithConnection<TData>(this MySqlConnection connection, bool isAtomic, Func<MySqlConnection, Result<TData>> action)
        => Result.AsResult(() =>
        {
            bool isOpenedByThis = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                isOpenedByThis = true;
            }
            var result = action(connection);
            if (isAtomic && isOpenedByThis)
            {
                connection.Close();
            }
            return result;
        });

    internal static async Task<Result<TData>> WithConnection<TData>(this MySqlConnection connection, bool isAtomic, Func<MySqlConnection, Task<Result<TData>>> action)
        => await Result.AsResult(async () =>
        {
            bool isOpenedByThis = false;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                isOpenedByThis = true;
            }
            var result = await action(connection);
            if (isAtomic && isOpenedByThis)
            {
                connection.Close();
            }
            return result;
        });

    internal static object? AdaptFromMysqlValue(this object? value, DataTypes dataType)
        => dataType switch
        {
            DataTypes.INTEGER => value as int?,
            DataTypes.LONG => value as long?,
            DataTypes.STRING => value as string,
            DataTypes.DECIMAL => value as double?,
            DataTypes.BOOLEAN => value as bool?,
            DataTypes.DATETIME => value is string str ? DateTime.Parse(str) : value as DateTime?,
            DataTypes.UNIT => null,
            _ => throw new NotImplementedException("Unknown data type")
        };

    internal static string ToMysqlTypeName(this DataTypes dataType)
        => dataType switch
        {
            DataTypes.INTEGER => "int",
            DataTypes.LONG => "bigint",
            DataTypes.STRING => "text",
            DataTypes.DECIMAL => "double",
            DataTypes.BOOLEAN => "tinyint(1)",
            DataTypes.DATETIME => "datetime",
            DataTypes.UNIT => "void",
            _ => throw new NotImplementedException("Unknown data type")
        };

    internal static DataTypes FromMysqlTypeName(this string typeName)
        => typeName.ToLower() switch
        {
            "int" => DataTypes.INTEGER,
            "bigint" => DataTypes.LONG,
            var type when type.Equals("text") || type.StartsWith("varchar") || type.StartsWith("char") => DataTypes.STRING,
            "double" => DataTypes.DECIMAL,
            "tinyint(1)" => DataTypes.BOOLEAN,
            var type when type.StartsWith("datetime") || type.StartsWith("date") || type.StartsWith("timestamp") => DataTypes.DATETIME,
            "void" => DataTypes.UNIT,
            _ => throw new NotImplementedException("Unknown data type")
        };
}
