using Bifrons.Lenses.Symmetric.Relational.Columns;
using Bifrons.Lenses.Symmetric.Relational.Data.Model;
using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Data;

public abstract class DataColumnLens<TLeftData, TRightData> 
    : ISymmetricLens<DataColumn<TLeftData>, DataColumn<TRightData>>
{
    protected readonly string _targetColumnName;
    protected readonly SymmetricColumnLens _columnLens;
    protected readonly ISymmetricLens<TLeftData, TRightData> _dataLens;

    public string TargetColumnName => _targetColumnName;
    public SymmetricColumnLens ColumnLens => _columnLens;
    public ISymmetricLens<TLeftData, TRightData> DataLens => _dataLens;

    protected DataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens)
    {
        _targetColumnName = targetColumnName;
        _columnLens = columnLens;
        _dataLens = dataLens;
    }

    public Func<DataColumn<TRightData>, Option<DataColumn<TLeftData>>, Result<DataColumn<TLeftData>>> PutLeft => throw new NotImplementedException();

    public Func<DataColumn<TLeftData>, Option<DataColumn<TRightData>>, Result<DataColumn<TRightData>>> PutRight => throw new NotImplementedException();

    public Func<DataColumn<TLeftData>, Result<DataColumn<TRightData>>> CreateRight => 
        source => _columnLens.CreateRight(source.Column)
            .Bind(rightColumn => _dataLens.CreateRight(source.Data)
                .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData)));
        //Result.Success(DataColumn<TRightData>.Cons(, _dataLens.CreateRight(source.Data).Data));

    public Func<DataColumn<TRightData>, Result<DataColumn<TLeftData>>> CreateLeft => throw new NotImplementedException();

}
