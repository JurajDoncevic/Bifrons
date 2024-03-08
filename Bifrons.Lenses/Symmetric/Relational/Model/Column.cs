namespace Bifrons.Lenses.Symmetric.Relational.Model;

public abstract class Column
{
    protected readonly string _name;
    protected readonly DataTypes _dataType;
    protected Type _type;
    public DataTypes DataType => _dataType;
    public string Name => _name;

    protected Column(string name, DataTypes dataType)
    {
        _name = name;
        _dataType = dataType;
        _type = ToType(dataType);
    }

    public static Type ToType(DataTypes dataType)
    {
        return dataType switch
        {
            DataTypes.STRING => typeof(string),
            DataTypes.INTEGER => typeof(int),
            DataTypes.DECIMAL => typeof(double),
            DataTypes.BOOLEAN => typeof(bool),
            DataTypes.DATETIME => typeof(DateTime),
            _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
        };
    }

    public static DataTypes ToDataType(Type type)
    {
        return type switch
        {
            Type t when t == typeof(string) => DataTypes.STRING,
            Type t when t == typeof(int) => DataTypes.INTEGER,
            Type t when t == typeof(double) => DataTypes.DECIMAL,
            Type t when t == typeof(bool) => DataTypes.BOOLEAN,
            Type t when t == typeof(DateTime) => DataTypes.DATETIME,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
