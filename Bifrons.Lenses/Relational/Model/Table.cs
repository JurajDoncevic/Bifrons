namespace Bifrons.Lenses.Relational.Model;

public sealed class Table
{
    public const string UNIT_NAME = "";
    private readonly string _name;
    private readonly List<Column> _columns;
    private readonly bool _isUnit;

    public string Name => _name;
    public IReadOnlyList<Column> Columns => _columns;
    public bool IsUnit => _isUnit;

    public Option<Column> this[string columnName] => _columns.FirstOrDefault(column => column.Name == columnName) ?? Option.None<Column>();

    private Table(string name, IEnumerable<Column> columns, bool isUnit = false)
    {
        _name = name;
        _columns = columns.ToList();
        _isUnit = isUnit;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Table table)
        {
            return table._name == _name && table._columns.SequenceEqual(_columns);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return _name.GetHashCode() ^ _columns.GetHashCode();
    }

    public override string ToString()
    {
        return $"({_name} : {string.Join(",\n ", _columns)})";
    }

    public static bool operator ==(Table left, Table right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Table left, Table right)
    {
        return !(left == right);
    }

    public static Table Cons(string name, IEnumerable<Column>? columns = null)
        => new(name, columns ?? []);

    public static Table Cons(string name, params Column[] columns)
        => new(name, columns ?? []);

    public static Table ConsUnit(string name = UNIT_NAME)
        => new(name, [], true);

}
