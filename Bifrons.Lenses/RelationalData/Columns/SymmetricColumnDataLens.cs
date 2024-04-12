using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public interface ISymmetricColumnDataLens
{
    string MatchesColumnNameLeft { get; }
    string MatchesColumnNameRight { get; }
    bool MatchesLeft { get; }
    bool MatchesRight { get; }
    DataTypes ForDataType { get; }
}

public abstract class SymmetricColumnDataLens<TLeftColumnData, TRightColumnData, TData>
    : ISymmetricLens<TLeftColumnData, TRightColumnData>,
      ISymmetricColumnDataLens
    where TLeftColumnData : ColumnData
    where TRightColumnData : ColumnData
{
    protected readonly SymmetricColumnLens _columnLens;
    protected readonly Option<ISymmetricLens<TData, TData>> _dataLens;

    public SymmetricColumnLens ColumnLens => _columnLens;
    public Option<ISymmetricLens<TData, TData>> DataLens => _dataLens;


    public string MatchesColumnNameLeft => _columnLens.MatchesColumnNameLeft;

    public string MatchesColumnNameRight => _columnLens.MatchesColumnNameRight;

    public bool MatchesLeft => _columnLens.MatchesLeft;

    public bool MatchesRight => _columnLens.MatchesRight;

    public abstract DataTypes ForDataType { get; }

    protected SymmetricColumnDataLens(SymmetricColumnLens columnLens, Option<ISymmetricLens<TData, TData>> dataLens)
    {
        _columnLens = columnLens;
        _dataLens = dataLens;
    }

    public abstract Func<TRightColumnData, Option<TLeftColumnData>, Result<TLeftColumnData>> PutLeft { get; }
    public abstract Func<TLeftColumnData, Option<TRightColumnData>, Result<TRightColumnData>> PutRight { get; }
    public abstract Func<TLeftColumnData, Result<TRightColumnData>> CreateRight { get; }
    public abstract Func<TRightColumnData, Result<TLeftColumnData>> CreateLeft { get; }
}
