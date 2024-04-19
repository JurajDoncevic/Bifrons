using Bifrons.Lenses.Relational.Model;
using Microsoft.Data.Sqlite;

namespace Bifrons.Cannonizers.Relational.Sqlite;

public static class Utils
{
    public enum SqliteDataTypes
    {
        INTEGER,
        TEXT,
        REAL,
        DATE,
        DATETIME,
        NULL
    }

    public static SqliteDataTypes FromDbString(string dbTypeName)
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

    public static SqliteDataTypes ToSqliteType(this DataTypes dataType)
    {
        return dataType switch
        {
            DataTypes.INTEGER => SqliteDataTypes.INTEGER,
            DataTypes.STRING => SqliteDataTypes.TEXT,
            DataTypes.DECIMAL => SqliteDataTypes.REAL,
            DataTypes.BOOLEAN => SqliteDataTypes.INTEGER,
            DataTypes.DATETIME => SqliteDataTypes.DATETIME,
            DataTypes.UNIT => SqliteDataTypes.NULL,
            _ => throw new NotImplementedException("Unknown data type")
        };
    }

    public static DataTypes ToDataType(this SqliteDataTypes sqliteType)
    {
        return sqliteType switch
        {
            SqliteDataTypes.INTEGER => DataTypes.INTEGER,
            SqliteDataTypes.TEXT => DataTypes.STRING,
            SqliteDataTypes.REAL => DataTypes.DECIMAL,
            SqliteDataTypes.DATE => DataTypes.DATETIME,
            SqliteDataTypes.DATETIME => DataTypes.DATETIME,
            SqliteDataTypes.NULL => DataTypes.UNIT,
            _ => throw new NotImplementedException("Unknown SQLite data type")
        };
    }

    public static Result<TData> WithConnection<TData>(this SqliteConnection connection, bool isAtomic, Func<SqliteConnection, Result<TData>> action)
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
