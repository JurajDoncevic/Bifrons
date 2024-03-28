using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Data.Model;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Data.Columns;

public interface IDataColumnLens 
{
    string TargetColumnName { get; }
    SymmetricColumnLens ColumnLens { get; }
    DataTypes LeftDataType { get; }
    DataTypes RightDataType { get; }
    Type LeftType { get; }
    Type RightType { get; }

}

public abstract class DataColumnLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData, TColumnLens> 
    : ISymmetricLens<TLeftDataColumn, TRightDataColumn>,
      IDataColumnLens
    where TColumnLens : notnull, SymmetricColumnLens
    where TLeftDataColumn : notnull, DataColumn<TLeftData>
    where TRightDataColumn : notnull, DataColumn<TRightData>
    where TLeftData : notnull
    where TRightData : notnull
{
    protected readonly string _targetColumnName;
    protected readonly SymmetricColumnLens _columnLens;
    protected readonly ISymmetricLens<TLeftData, TRightData> _dataLens;

    public string TargetColumnName => _targetColumnName;
    public SymmetricColumnLens ColumnLens => _columnLens;
    public ISymmetricLens<TLeftData, TRightData> DataLens => _dataLens;

    public DataTypes LeftDataType => typeof(TLeftData).ToDataType();

    public DataTypes RightDataType => typeof(TRightData).ToDataType();

    public Type LeftType => typeof(TLeftData);

    public Type RightType => typeof(TRightData);

    public abstract Func<TRightDataColumn, Option<TLeftDataColumn>, Result<TLeftDataColumn>> PutLeft { get; }
    public abstract Func<TLeftDataColumn, Option<TRightDataColumn>, Result<TRightDataColumn>> PutRight { get; }
    public abstract Func<TLeftDataColumn, Result<TRightDataColumn>> CreateRight { get; }
    public abstract Func<TRightDataColumn, Result<TLeftDataColumn>> CreateLeft { get; }

    protected DataColumnLens(string targetColumnName, TColumnLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens)
    {
        _targetColumnName = targetColumnName;
        _columnLens = columnLens;
        _dataLens = dataLens;
    }
}

