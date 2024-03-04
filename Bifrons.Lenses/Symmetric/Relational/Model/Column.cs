namespace Bifrons.Lenses.Relational.Model;

public abstract class Column<TData>
{
    protected readonly string _name;
    protected readonly DataTypes _dataType;
    protected readonly TData _data;
    public DataTypes DataType => _dataType;
    public string Name => _name;
    public TData Data => _data;

    protected Column(string name, DataTypes dataType, TData data)
    {
        _name = name;
        _dataType = dataType;
        _data = data;
    }
}
