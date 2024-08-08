using Bifrons.Lenses.Relational.Model;
using Npgsql;

namespace Bifrons.Cannonizers.Relational.Postgres;

internal static class Utils
{

    internal static Result<TData> WithConnection<TData>(this NpgsqlConnection connection, bool isAtomic, Func<NpgsqlConnection, Result<TData>> action)
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

    internal static async Task<Result<TData>> WithConnection<TData>(this NpgsqlConnection connection, bool isAtomic, Func<NpgsqlConnection, Task<Result<TData>>> action)
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

    internal static object? AdaptFromPostgresValue(this object? value, DataTypes dataType)
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

    internal static string ToPostgresTypeName(this DataTypes dataType)
        => dataType switch
        {
            DataTypes.INTEGER => "integer",
            DataTypes.LONG => "bigint",
            DataTypes.STRING => "text",
            DataTypes.DECIMAL => "double precision",
            DataTypes.BOOLEAN => "boolean",
            DataTypes.DATETIME => "timestamp",
            DataTypes.UNIT => "void",
            _ => throw new NotImplementedException("Unknown data type")
        };

    internal static DataTypes FromPostgresTypeName(this string typeName)
        => typeName.ToLower() switch
        {
            "integer" => DataTypes.INTEGER,
            "bigint" => DataTypes.LONG,
            var str when str.Contains("text") || str.Contains("char") => DataTypes.STRING,
            "double precision" => DataTypes.DECIMAL,
            "boolean" => DataTypes.BOOLEAN,
            var dt when dt.StartsWith("time") || dt.StartsWith("date") => DataTypes.DATETIME,
            "void" => DataTypes.UNIT,
            _ => throw new NotImplementedException("Unknown data type")
        };
}
