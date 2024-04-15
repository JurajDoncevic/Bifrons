using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.RelationalData.Model;

public sealed class TableData
{
    private readonly string _name;
    private List<RowData> _rowData;
    private readonly Table _table;

    public string Name => _name;
    public IReadOnlyList<RowData> RowData => _rowData;
    public IReadOnlyList<Column> Columns => _table.Columns;
    public Table Table => _table;
    public bool IsUnit => _table.IsUnit;

    private TableData(Table table, IEnumerable<RowData> rowData)
    {
        _name = table.Name;
        _rowData = rowData.ToList();
        _table = rowData.Count() > 0 ? Table.Cons(_name, _rowData.First().Columns) : Table.ConsUnit(_name);
    }

    public override bool Equals(object? obj)
        => obj is not null
            && obj is TableData
            && _name.Equals((obj as TableData)!.Name)
            && _rowData.SequenceEqual((obj as TableData)!.RowData);

    public override int GetHashCode()
        => HashCode.Combine(_name, _rowData);

    public static Result<TableData> Cons(Table table, IEnumerable<RowData>? rowData = null)
        => rowData?.All(rd => rd.IsQualifiablyEqualType(rowData.First())) ?? true
           && table.Columns.All(c => rowData?.All(rd => rd.Columns.Any(cd => cd.Name == c.Name)) ?? true)
            ? Result.Success(new TableData(table, rowData ?? []))
            : Result.Failure<TableData>("Row data provided is not full-qualifiably type compatible.");

    public static Result<TableData> ConsUnit(string? name = null)
        => Result.Success(new TableData(Table.ConsUnit(name), []));
}
