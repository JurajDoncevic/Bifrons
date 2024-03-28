namespace Bifrons.Lenses.Relational.Model;

public enum DataTypes
{
    INTEGER,
    DECIMAL,
    STRING,
    BOOLEAN,
    DATETIME,
    UNIT
}

public static class DataTypesExtensions 
{
    public static Type ToType(this DataTypes dataType)
    {
        return dataType switch
        {
            DataTypes.STRING => typeof(string),
            DataTypes.INTEGER => typeof(int),
            DataTypes.DECIMAL => typeof(double),
            DataTypes.BOOLEAN => typeof(bool),
            DataTypes.DATETIME => typeof(DateTime),
            DataTypes.UNIT => typeof(Unit),
            _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
        };
    }

    public static DataTypes ToDataType(this Type type)
    {
        return type switch
        {
            Type t when t == typeof(string) => DataTypes.STRING,
            Type t when t == typeof(int) => DataTypes.INTEGER,
            Type t when t == typeof(double) => DataTypes.DECIMAL,
            Type t when t == typeof(bool) => DataTypes.BOOLEAN,
            Type t when t == typeof(DateTime) => DataTypes.DATETIME,
            Type t when t == typeof(Unit) => DataTypes.UNIT,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
