using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public interface ISymmetricDataColumnLens
{
    string MatchesColumnNameLeft { get; }
    string MatchesColumnNameRight { get; }
    bool MatchesLeft { get; }
    bool MatchesRight { get; }
    DataTypes ForDataType { get; }
}

public abstract class SymmetricDataColumnLens<TLeftDataColumn, TLeftData, TLeftColumn, TRightDataColumn, TRightData, TRightColumn>
    : ISymmetricLens<TLeftDataColumn, TRightDataColumn>, ISymmetricDataColumnLens
    where TLeftDataColumn : IDataColumn<TLeftData, TLeftColumn>, DataColumn
    where TRightDataColumn : IDataColumn<TRightData, TRightColumn>, DataColumn
    where TLeftColumn : Column, IColumn<TLeftData>
    where TRightColumn : Column, IColumn<TRightData>
{
    protected readonly SymmetricColumnLens _columnLens;
    protected readonly ISymmetricLens<TLeftData, TRightData> _dataLens;

    public abstract DataTypes ForDataType { get; }

    public string MatchesColumnNameLeft => _columnLens.MatchesColumnNameLeft;

    public string MatchesColumnNameRight => _columnLens.MatchesColumnNameRight;

    public bool MatchesLeft => _columnLens.MatchesLeft;

    public bool MatchesRight => _columnLens.MatchesRight;

    protected SymmetricDataColumnLens(SymmetricColumnLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens)
    {
        _columnLens = columnLens;
        _dataLens = dataLens;
    }

    public abstract Func<TRightDataColumn, Option<TLeftDataColumn>, Result<TLeftDataColumn>> PutLeft { get; }
    public abstract Func<TLeftDataColumn, Option<TRightDataColumn>, Result<TRightDataColumn>> PutRight { get; }
    public abstract Func<TLeftDataColumn, Result<TRightDataColumn>> CreateRight { get; }
    public abstract Func<TRightDataColumn, Result<TLeftDataColumn>> CreateLeft { get; }
}
