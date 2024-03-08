namespace Bifrons.Lenses.Symmetric.Relational.Model;

public sealed class Table
{
    private readonly string _name;
    private readonly List<Column> _columns;

    public string Name => _name;

    public IReadOnlyList<Column> Columns => _columns;

    public Table(string name, List<Column>? columns = null)
    {
        _name = name;
        _columns = columns ?? new List<Column>();
    }
}
