namespace Bifrons.Lenses.Relational.Model;

/// <summary>
/// Represents a table in a relational database.
/// </summary>
public sealed class Table
{
    /// <summary>
    /// Default name for the unit table
    /// </summary>
    public const string UNIT_NAME = "";

    private readonly string _name;
    private readonly List<Column> _columns;
    private readonly bool _isUnit;

    /// <summary>
    /// Table name
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Columns in the table
    /// </summary>
    public IReadOnlyList<Column> Columns => _columns;

    /// <summary>
    /// Whether the table is a unit table. A placehoder for a table with no columns (e.g. a deleted table).
    /// </summary>
    public bool IsUnit => _isUnit;

    /// <summary>
    /// Column indexer
    /// </summary>
    public Option<Column> this[string columnName] => _columns.FirstOrDefault(column => column.Name == columnName) ?? Option.None<Column>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Table name</param>
    /// <param name="columns">Columns in the table</param>
    /// <param name="isUnit">Whether the table is a unit table</param>
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

    /// <summary>
    /// Constructs a new table
    /// </summary>
    /// <param name="name">Table name</param>
    /// <param name="columns">Columns in the table</param> 
    public static Table Cons(string name, IEnumerable<Column>? columns = null)
        => new(name, columns ?? []);

    /// <summary>
    /// Constructs a new table
    /// </summary>
    /// <param name="name">Table name</param>
    /// <param name="columns">Columns in the table</param>
    public static Table Cons(string name, params Column[] columns)
        => new(name, columns ?? []);

    /// <summary>
    /// Constructs a new unit table
    /// </summary>
    /// <param name="name">Table name</param>
    public static Table ConsUnit(string? name = UNIT_NAME)
        => new(name ?? UNIT_NAME, [], true);

}
