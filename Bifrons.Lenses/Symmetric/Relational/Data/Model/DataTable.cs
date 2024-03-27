using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Data.Model;

public sealed class DataTable
{
    private readonly Table _table;
    private readonly List<IDataColumn> _columns;

    public Table Table => _table;
    public IReadOnlyList<IDataColumn> Columns => _columns;

    private DataTable(Table table, IEnumerable<IDataColumn> columns)
    {
        _table = table;
        _columns = columns.ToList();
    }

    public static DataTable Cons(Table table, IEnumerable<IDataColumn>? columns = null) 
        => new(table, columns ?? []);
}
