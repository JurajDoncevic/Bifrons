namespace Bifrons.Lenses.RelationalData.Model;

public sealed class TabularData
{
    private readonly string _name;
    private List<RowData> _rowData;

    public string Name => _name;
    public IReadOnlyList<RowData> RowData => _rowData;

    private TabularData(string name, IEnumerable<RowData> rowData)
    {
        _name = name;
        _rowData = rowData.ToList();
    }

    public override bool Equals(object? obj)
        => obj is not null
            && obj is TabularData
            && _name.Equals((obj as TabularData)!.Name)
            && _rowData.SequenceEqual((obj as TabularData)!.RowData);

    public override int GetHashCode()
        => HashCode.Combine(_name, _rowData);

    public static Result<TabularData> Cons(string name, IEnumerable<RowData>? rowData = null)
        => rowData?.All(rd => rd.IsQualifiablyEqualType(rowData.First())) ?? true
            ? Result.Success(new TabularData(name, rowData ?? []))
            : Result.Failure<TabularData>("Row data provided is not full-qualifiably type compatible.");
}
