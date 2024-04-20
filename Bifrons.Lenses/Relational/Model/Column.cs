namespace Bifrons.Lenses.Relational.Model;

/// <summary>
/// Represents a column in a table.
/// </summary>
public abstract class Column
{
    protected readonly string _name;
    protected readonly DataTypes _dataType;
    protected Type _type;
    public DataTypes DataType => _dataType;
    public string Name => _name;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name"><Column name/param>
    /// <param name="dataType">Column data type</param>
    protected Column(string name, DataTypes dataType)
    {
        _name = name;
        _dataType = dataType;
        _type = dataType.ToType();
    }

    /// <summary>
    /// Constructs a concrete column type based on the data type.
    /// </summary>
    public static Column Cons(string name, DataTypes dataType) => dataType switch
    {
        DataTypes.STRING => StringColumn.Cons(name),
        DataTypes.INTEGER => IntegerColumn.Cons(name),
        DataTypes.LONG => LongColumn.Cons(name),
        DataTypes.DECIMAL => DecimalColumn.Cons(name),
        DataTypes.BOOLEAN => BooleanColumn.Cons(name),
        DataTypes.DATETIME => DateTimeColumn.Cons(name),
        DataTypes.UNIT => UnitColumn.Cons(name),
        _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
    };

    public override string ToString()
    {
        return $"({_name} : {_dataType})";
    }
    public override bool Equals(object? obj)
    {
        if (obj is Column column)
        {
            return column._name == _name && column._dataType == _dataType;
        }
        return false;
    }

    public static bool operator ==(Column left, Column right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Column left, Column right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode() ^ _dataType.GetHashCode();
    }
}
