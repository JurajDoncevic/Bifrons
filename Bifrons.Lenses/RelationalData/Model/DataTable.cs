using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.RelationalData.Model;

public sealed class DataTable
{
    private readonly string _name;
    private readonly List<IDataColumn> _columns;

    public string Name => _name;
    public IReadOnlyList<IDataColumn> Columns => _columns;

    public IDataColumn? this[string columnName] => _columns.FirstOrDefault(column => column.Name == columnName);

    public Table Table => Table.Cons(_name, _columns.Map(dataColumn => Column.Cons(dataColumn.Name, dataColumn.DataType)));

    private DataTable(string tableName, IEnumerable<IDataColumn> columns)
    {
        _name = tableName;
        _columns = columns.ToList();
    }

    public static DataTable Cons(string tableName, IEnumerable<IDataColumn>? columns = null)
        => new(tableName, columns ?? []);

    public static DataTable Cons(string tableName, params IDataColumn[] columns)
        => new(tableName, columns ?? []);
}
