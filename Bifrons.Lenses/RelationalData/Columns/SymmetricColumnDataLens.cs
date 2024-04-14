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
    SymmetricColumnLens ColumnLens { get; }

    Func<ColumnData, Option<ColumnData>, Result<ColumnData>> PutLeft { get; }
    Func<ColumnData, Option<ColumnData>, Result<ColumnData>> PutRight { get; }
    Func<ColumnData, Result<ColumnData>> CreateRight { get; }
    Func<ColumnData, Result<ColumnData>> CreateLeft { get; }


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

    Func<ColumnData, Option<ColumnData>, Result<ColumnData>> ISymmetricColumnDataLens.PutLeft =>
        (updatedSource, originalTarget) => PutLeft((TRightColumnData)updatedSource, originalTarget.Map(_ => (TLeftColumnData)_)).Map(_ => (ColumnData)_);

    Func<ColumnData, Option<ColumnData>, Result<ColumnData>> ISymmetricColumnDataLens.PutRight => 
        (updatedSource, originalTarget) => PutRight((TLeftColumnData)updatedSource, originalTarget.Map(_ => (TRightColumnData)_)).Map(_ => (ColumnData)_);

    Func<ColumnData, Result<ColumnData>> ISymmetricColumnDataLens.CreateRight => 
        source => CreateRight((TLeftColumnData)source).Map(_ => (ColumnData)_);

    Func<ColumnData, Result<ColumnData>> ISymmetricColumnDataLens.CreateLeft => 
        source => CreateLeft((TRightColumnData)source).Map(_ => (ColumnData)_);
}
