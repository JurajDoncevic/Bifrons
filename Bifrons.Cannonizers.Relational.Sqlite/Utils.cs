using Bifrons.Lenses.Relational.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

internal static class Utils
{
    internal enum SqliteDataTypes
    {
        INTEGER,
        TEXT,
        REAL,
        DATE,
        DATETIME,
        NULL
    }

    internal static SqliteDataTypes FromDbString(string dbTypeName)
        => dbTypeName switch
        {
            "INTEGER" => SqliteDataTypes.INTEGER,
            "TEXT" => SqliteDataTypes.TEXT,
            "REAL" => SqliteDataTypes.REAL,
            "DATE" => SqliteDataTypes.DATE,
            "DATETIME" => SqliteDataTypes.DATETIME,
            "NULL" => SqliteDataTypes.NULL,
            _ => throw new NotImplementedException($"Unknown data type {dbTypeName}")
        };

    internal static SqliteDataTypes ToSqliteType(this DataTypes dataType)
    {
        return dataType switch
        {
            DataTypes.INTEGER => SqliteDataTypes.INTEGER,
            DataTypes.LONG => SqliteDataTypes.INTEGER,
            DataTypes.STRING => SqliteDataTypes.TEXT,
            DataTypes.DECIMAL => SqliteDataTypes.REAL,
            DataTypes.BOOLEAN => SqliteDataTypes.INTEGER,
            DataTypes.DATETIME => SqliteDataTypes.DATETIME,
            DataTypes.UNIT => SqliteDataTypes.NULL,
            _ => throw new NotImplementedException("Unknown data type")
        };
    }

    internal static DataTypes ToDataType(this SqliteDataTypes sqliteType)
    {
        return sqliteType switch
        {
            SqliteDataTypes.INTEGER => DataTypes.LONG,
            SqliteDataTypes.TEXT => DataTypes.STRING,
            SqliteDataTypes.REAL => DataTypes.DECIMAL,
            SqliteDataTypes.DATE => DataTypes.DATETIME,
            SqliteDataTypes.DATETIME => DataTypes.DATETIME,
            SqliteDataTypes.NULL => DataTypes.UNIT,
            _ => throw new NotImplementedException("Unknown SQLite data type")
        };
    }

    internal static object? AdaptFromSqliteValue(this object? value, DataTypes dataType)
    {
        return dataType switch
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
    }

    internal static object? AdaptToSqliteValue(this object? value, DataTypes dataType)
    {
        return dataType switch
        {
            DataTypes.INTEGER => value as int?,
            DataTypes.LONG => value as long?,
            DataTypes.STRING => value is not null ? $"\"{value}\"" : value as string,
            DataTypes.DECIMAL => value as double?,
            DataTypes.BOOLEAN => value is not null ? ((bool) value ? 1 : 0) : value as int?,
            DataTypes.DATETIME => value is DateTime dt ? $"\"{dt.ToString("yyyy-MM-dd HH:mm:ss.fff")}\"" : "NULL",
            DataTypes.UNIT => null,
            _ => throw new NotImplementedException("Unknown data type")
        };
    }

    internal static Result<TData> WithConnection<TData>(this SqliteConnection connection, bool isAtomic, Func<SqliteConnection, Result<TData>> action)
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
}
