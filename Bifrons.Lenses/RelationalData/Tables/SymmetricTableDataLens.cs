using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Relational.Tables;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public abstract class SymmetricTableDataLens
    : ISymmetricLens<TableData, TableData>
{
    protected readonly SymmetricTableLens _tableLens;
    protected readonly List<ISymmetricColumnDataLens> _columnDataLenses;

    public SymmetricTableLens TableLens => _tableLens;
    public IReadOnlyList<ISymmetricColumnDataLens> ColumnDataLenses => _columnDataLenses;
    public string MatchesColumnNameLeft => _tableLens.MatchesTableNameLeft;
    public string MatchesColumnNameRight => _tableLens.MatchesTableNameRight;
    public bool MatchesLeft => _tableLens.MatchesLeft;
    public bool MatchesRight => _tableLens.MatchesRight;

    public Option<ISymmetricColumnDataLens> this[string name] => _columnDataLenses.FirstOrDefault(cdl => cdl.MatchesColumnNameLeft == name).ToOption();

    protected SymmetricTableDataLens(SymmetricTableLens tableLens, IEnumerable<ISymmetricColumnDataLens> columnDataLenses)
    {
        _tableLens = tableLens;
        _columnDataLenses = columnDataLenses.ToList();
    }

    public abstract Func<TableData, Option<TableData>, Result<TableData>> PutLeft { get; }
    public abstract Func<TableData, Option<TableData>, Result<TableData>> PutRight { get; }
    public abstract Func<TableData, Result<TableData>> CreateRight { get; }
    public abstract Func<TableData, Result<TableData>> CreateLeft { get; }
}
