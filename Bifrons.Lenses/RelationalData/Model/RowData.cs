using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.RelationalData.Model;

public sealed class RowData
{
    private List<ColumnData> _columnData;

    public IReadOnlyList<ColumnData> ColumnData => _columnData;

    public IReadOnlyList<Column> Columns => _columnData.Map(cd => cd.Column).ToList();

    public Option<ColumnData> this[string name] => _columnData.FirstOrDefault(cd => cd.Name == name) ?? Option.None<ColumnData>();

    private RowData(IEnumerable<ColumnData> columnData)
    {
        _columnData = columnData.ToList();
    }

    public override bool Equals(object? obj)
        => obj is not null
            && obj is RowData
            && Equals((obj as RowData)!);

    private bool Equals(RowData other)
        => _columnData.SequenceEqual(other._columnData);

    public override int GetHashCode()
        => HashCode.Combine(_columnData);

    /// <summary>
    /// Returns true if the two RowData instances have the same number of columns and the ordered columns have the same data types.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEqualType(RowData other)
        => _columnData.Count == other._columnData.Count
            && _columnData.Zip(other._columnData, (cd1, cd2) => cd1.DataType == cd2.DataType).All(b => b);
    
    /// <summary>
    /// Returns true if the two RowData instances have the same number of columns and the ordered columns have the same data types and names.
    /// </summary>
    /// <param name="other"></param>
    public bool IsQualifiablyEqualType(RowData other)
        => _columnData.Count == other._columnData.Count
            && _columnData.Zip(other._columnData, (cd1, cd2) => cd1.DataType == cd2.DataType && cd1.Name.Equals(cd2.Name)).All(b => b);

    /// <summary>
    /// Constructs a RowData instance from the given column data.
    /// </summary>
    /// <param name="columnData">Column data</param>
    public static RowData Cons(IEnumerable<ColumnData> columnData)
        => new(columnData);
}

public static class RowDataExtensions
{
    public static RowData Concat(this RowData left, RowData right)
        => RowData.Cons(left.ColumnData.Concat(right.ColumnData));
}
