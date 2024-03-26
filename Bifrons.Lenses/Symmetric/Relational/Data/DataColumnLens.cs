using Bifrons.Lenses.Symmetric.Relational.Columns;
using Bifrons.Lenses.Symmetric.Relational.Data.Model;
using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Data;

public abstract class DataColumnLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData> 
    : ISymmetricLens<TLeftDataColumn, TRightDataColumn>
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

    protected DataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens)
    {
        _targetColumnName = targetColumnName;
        _columnLens = columnLens;
        _dataLens = dataLens;
    }

    public Func<DataColumn<TRightData>, Option<DataColumn<TLeftData>>, Result<DataColumn<TLeftData>>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, Option.Some(target.Column))
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.Some(target.Data))
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData))),
            () => _columnLens.PutLeft(updatedSource.Column, Option.None<Column>())
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.None<TLeftData>())
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData)))
        );

    public Func<DataColumn<TLeftData>, Option<DataColumn<TRightData>>, Result<DataColumn<TRightData>>> PutRight =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, Option.Some(target.Column))
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.Some(target.Data))
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData))),
            () => _columnLens.PutRight(updatedSource.Column, Option.None<Column>())
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.None<TRightData>())
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData)))
        );

    public Func<DataColumn<TLeftData>, Result<DataColumn<TRightData>>> CreateRight => 
        source => _columnLens.CreateRight(source.Column)
            .Bind(rightColumn => _dataLens.CreateRight(source.Data)
                .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData)));

    public Func<DataColumn<TRightData>, Result<DataColumn<TLeftData>>> CreateLeft => 
        target => _columnLens.CreateLeft(target.Column)
            .Bind(leftColumn => _dataLens.CreateLeft(target.Data)
                .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData)));

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    Func<TRightDataColumn, Option<TLeftDataColumn>, Result<TLeftDataColumn>> ISymmetricLens<TLeftDataColumn, TRightDataColumn>.PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, Option.Some(target.Column))
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.Some(target.Data))
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn)),
            () => _columnLens.PutLeft(updatedSource.Column, Option.None<Column>())
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.None<TLeftData>())
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn))
        );

    Func<TLeftDataColumn, Option<TRightDataColumn>, Result<TRightDataColumn>> ISymmetricLens<TLeftDataColumn, TRightDataColumn>.PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, Option.Some(target.Column))
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.Some(target.Data))
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn)),
            () => _columnLens.PutRight(updatedSource.Column, Option.None<Column>())
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.None<TRightData>())
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn))
        );

    Func<TLeftDataColumn, Result<TRightDataColumn>> ISymmetricLens<TLeftDataColumn, TRightDataColumn>.CreateRight => 
        source => _columnLens.CreateRight(source.Column)
            .Bind(rightColumn => _dataLens.CreateRight(source.Data)
                .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn));

    Func<TRightDataColumn, Result<TLeftDataColumn>> ISymmetricLens<TLeftDataColumn, TRightDataColumn>.CreateLeft => 
        source => _columnLens.CreateLeft(source.Column)
            .Bind(leftColumn => _dataLens.CreateLeft(source.Data)
                .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
}


public sealed class IntegerDataColumnLens : DataColumnLens<IntegerDataColumn, IntegerDataColumn, int, int>
{
    public IntegerDataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<int, int> dataLens) 
        : base(targetColumnName, columnLens, dataLens)
    {
    }
}

public sealed class StringDataColumnLens : DataColumnLens<StringDataColumn, StringDataColumn, string, string>
{
    public StringDataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<string, string> dataLens) 
        : base(targetColumnName, columnLens, dataLens)
    {
    }
}

public sealed class BooleanDataColumnLens : DataColumnLens<BooleanDataColumn, BooleanDataColumn, bool, bool>
{
    public BooleanDataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<bool, bool> dataLens) 
        : base(targetColumnName, columnLens, dataLens)
    {
    }
}

public sealed class DateTimeDataColumnLens : DataColumnLens<DateTimeDataColumn, DateTimeDataColumn, DateTime, DateTime>
{
    public DateTimeDataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) 
        : base(targetColumnName, columnLens, dataLens)
    {
    }
}

public sealed class DecimalDataColumnLens : DataColumnLens<DecimalDataColumn, DecimalDataColumn, double, double>
{
    public DecimalDataColumnLens(string targetColumnName, SymmetricColumnLens columnLens, ISymmetricLens<double, double> dataLens) 
        : base(targetColumnName, columnLens, dataLens)
    {
    }
}