using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Data.Model;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Data.Columns;

public abstract class DeleteLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData>
    : DataColumnLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData, DeleteLens>
    where TLeftDataColumn : notnull, DataColumn<TLeftData>
    where TRightDataColumn : notnull, DataColumn<TRightData>
    where TLeftData : notnull
    where TRightData : notnull
{
    protected DeleteLens(string targetColumnName, DeleteLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public override Func<TRightDataColumn, Option<TLeftDataColumn>, Result<TLeftDataColumn>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, Option.Some(target.Column))
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.Some(target.Data))
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn)),
            () => _columnLens.PutLeft(updatedSource.Column, Option.None<Column>())
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.None<TLeftData>())
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn)
                )
        )!;

    public override Func<TLeftDataColumn, Option<TRightDataColumn>, Result<TRightDataColumn>> PutRight =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, Option.Some(target.Column))
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.Some(target.Data))
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn)),
            () => _columnLens.PutRight(updatedSource.Column, Option.None<Column>())
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.None<TRightData>())
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn)
                )
        )!;

    public override Func<TLeftDataColumn, Result<TRightDataColumn>> CreateRight =>
        source => _columnLens.CreateRight(source.Column)
            .Bind(rightColumn => _dataLens.CreateRight(source.Data)
                .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn))!;

    public override Func<TRightDataColumn, Result<TLeftDataColumn>> CreateLeft =>
        source => _columnLens.CreateLeft(source.Column)
            .Bind(leftColumn => _dataLens.CreateLeft(source.Data)
                .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn))!;
}

public sealed class IntegerDeleteLens : DeleteLens<IntegerDataColumn, IntegerDataColumn, int, int>
{
    private IntegerDeleteLens(string targetColumnName, DeleteLens columnLens, ISymmetricLens<int, int> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static IntegerDeleteLens Cons(string targetColumnName, DeleteLens columnLens, ISymmetricLens<int, int> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class StringDeleteLens : DeleteLens<StringDataColumn, StringDataColumn, string, string>
{
    private StringDeleteLens(string targetColumnName, DeleteLens columnLens, ISymmetricLens<string, string> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static StringDeleteLens Cons(string targetColumnName, DeleteLens columnLens, ISymmetricLens<string, string> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class DateTimeDeleteLens : DeleteLens<DateTimeDataColumn, DateTimeDataColumn, DateTime, DateTime>
{
    private DateTimeDeleteLens(string targetColumnName, DeleteLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static DateTimeDeleteLens Cons(string targetColumnName, DeleteLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class BooleanDeleteLens : DeleteLens<BooleanDataColumn, BooleanDataColumn, bool, bool>
{
    private BooleanDeleteLens(string targetColumnName, DeleteLens columnLens, ISymmetricLens<bool, bool> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static BooleanDeleteLens Cons(string targetColumnName, DeleteLens columnLens, ISymmetricLens<bool, bool> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class DecimalDeleteLens : DeleteLens<DecimalDataColumn, DecimalDataColumn, double, double>
{
    private DecimalDeleteLens(string targetColumnName, DeleteLens columnLens, ISymmetricLens<double, double> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static DecimalDeleteLens Cons(string targetColumnName, DeleteLens columnLens, ISymmetricLens<double, double> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}