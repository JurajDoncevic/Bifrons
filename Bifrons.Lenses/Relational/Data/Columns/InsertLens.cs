using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Data.Model;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Data.Columns;

public abstract class InsertLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData>
    : DataColumnLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData, InsertLens>
    where TLeftDataColumn : notnull, DataColumn<TLeftData>
    where TRightDataColumn : notnull, DataColumn<TRightData>
    where TLeftData : notnull
    where TRightData : notnull
{
    protected InsertLens(string targetColumnName, InsertLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens) : base(targetColumnName, columnLens, dataLens)
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


public sealed class IntegerInsertLens : InsertLens<IntegerDataColumn, IntegerDataColumn, int, int>
{
    private IntegerInsertLens(string targetColumnName, InsertLens columnLens, ISymmetricLens<int, int> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static IntegerInsertLens Cons(string targetColumnName, InsertLens columnLens, ISymmetricLens<int, int> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class StringInsertLens : InsertLens<StringDataColumn, StringDataColumn, string, string>
{
    private StringInsertLens(string targetColumnName, InsertLens columnLens, ISymmetricLens<string, string> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static StringInsertLens Cons(string targetColumnName, InsertLens columnLens, ISymmetricLens<string, string> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class DateTimeInsertLens : InsertLens<DateTimeDataColumn, DateTimeDataColumn, DateTime, DateTime>
{
    private DateTimeInsertLens(string targetColumnName, InsertLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static DateTimeInsertLens Cons(string targetColumnName, InsertLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class BooleanInsertLens : InsertLens<BooleanDataColumn, BooleanDataColumn, bool, bool>
{
    private BooleanInsertLens(string targetColumnName, InsertLens columnLens, ISymmetricLens<bool, bool> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static BooleanInsertLens Cons(string targetColumnName, InsertLens columnLens, ISymmetricLens<bool, bool> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class DecimalInsertLens : InsertLens<DecimalDataColumn, DecimalDataColumn, double, double>
{
    private DecimalInsertLens(string targetColumnName, InsertLens columnLens, ISymmetricLens<double, double> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static DecimalInsertLens Cons(string targetColumnName, InsertLens columnLens, ISymmetricLens<double, double> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}