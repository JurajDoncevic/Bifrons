using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class SymmetricDataColumnLens<TLeftDataColumn, TLeftData, TLeftColumn, TRightDataColumn, TRightData, TRightColumn>
    : ISymmetricLens<TLeftDataColumn, TRightDataColumn>
    where TLeftDataColumn : DataColumn<TLeftData, TLeftColumn>
    where TRightDataColumn : DataColumn<TRightData, TRightColumn>
    where TLeftColumn : Column, IColumn<TLeftData>
    where TRightColumn : Column, IColumn<TRightData>
{
    protected readonly SymmetricColumnLens _columnLens;
    protected readonly ISymmetricLens<TLeftData, TRightData> _dataLens;

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
