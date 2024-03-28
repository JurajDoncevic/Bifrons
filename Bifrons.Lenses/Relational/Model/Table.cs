namespace Bifrons.Lenses.Relational.Model;

public sealed class Table
{
    private readonly string _name;
    private readonly List<Column> _columns;

    public string Name => _name;

    public IReadOnlyList<Column> Columns => _columns;

    private Table(string name, IEnumerable<Column> columns)
    {
        _name = name;
        _columns = columns.ToList();
    }

    public static Table Cons(string name, IEnumerable<Column>? columns = null)
        => new(name, columns ?? []);
}
