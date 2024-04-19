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
        NULL
    }

    public static SqliteDataTypes FromDbString(string dbTypeName)
        => dbTypeName switch
        {
            "INTEGER" => SqliteDataTypes.INTEGER,
            "TEXT" => SqliteDataTypes.TEXT,
            "REAL" => SqliteDataTypes.REAL,
            "NULL" => SqliteDataTypes.NULL,
            _ => throw new NotImplementedException()
        };

    public static SqliteDataTypes ToSqliteType(this DataTypes dataType)
    {
        return dataType switch
        {
            DataTypes.INTEGER => SqliteDataTypes.INTEGER,
            DataTypes.STRING => SqliteDataTypes.TEXT,
            DataTypes.DECIMAL => SqliteDataTypes.REAL,
            DataTypes.BOOLEAN => SqliteDataTypes.INTEGER,
            DataTypes.DATETIME => SqliteDataTypes.TEXT,
            DataTypes.UNIT => SqliteDataTypes.NULL,
            _ => throw new NotImplementedException()
        };
    }

    public static DataTypes ToDataType(this SqliteDataTypes sqliteType)
    {
        return sqliteType switch
        {
            SqliteDataTypes.INTEGER => DataTypes.INTEGER,
            SqliteDataTypes.TEXT => DataTypes.STRING,
            SqliteDataTypes.REAL => DataTypes.DECIMAL,
            SqliteDataTypes.NULL => DataTypes.UNIT,
            _ => throw new NotImplementedException()
        };
    }

    public static Result<TData> WithConnection<TData>(this SqliteConnection connection, bool isAtomic, Func<SqliteConnection, Result<TData>> action)
        => Result.AsResult(() =>
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            var result = action(connection);
            if (isAtomic)
            {
                connection.Close();
            }
            return result;
        });
}
