using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Relational.Tables;
using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public abstract class SymmetricTableDataLens
    : ISymmetricLens<TableData, TableData>
{
    protected readonly SymmetricTableLens _tableLens;
    protected readonly Option<List<ISymmetricColumnDataLens>> _columnDataLenses;

    public SymmetricTableLens TableLens => _tableLens;
    public IReadOnlyList<ISymmetricColumnDataLens> ColumnDataLenses => _columnDataLenses.Match(some => some, () => []);
    public string MatchesColumnNameLeft => _tableLens.MatchesTableNameLeft;
    public string MatchesColumnNameRight => _tableLens.MatchesTableNameRight;
    public bool MatchesLeft => _tableLens.MatchesLeft;
    public bool MatchesRight => _tableLens.MatchesRight;

    public Option<ISymmetricColumnDataLens> GetLensMatchingLeft(string columnName) => _columnDataLenses.Value.FirstOrDefault(cdl => cdl.MatchesColumnNameLeft == columnName).ToOption();
    public Option<ISymmetricColumnDataLens> GetLensMatchingRight(string columnName) => _columnDataLenses.Value.FirstOrDefault(cdl => cdl.MatchesColumnNameRight == columnName).ToOption();

    protected SymmetricTableDataLens(SymmetricTableLens tableLens, Option<IEnumerable<ISymmetricColumnDataLens>> columnDataLenses)
    {
        _tableLens = tableLens;
        _columnDataLenses = columnDataLenses.Map(cdls => cdls.ToList());
    }

    public abstract Func<TableData, Option<TableData>, Result<TableData>> PutLeft { get; }
    public abstract Func<TableData, Option<TableData>, Result<TableData>> PutRight { get; }
    public abstract Func<TableData, Result<TableData>> CreateRight { get; }
    public abstract Func<TableData, Result<TableData>> CreateLeft { get; }
}
