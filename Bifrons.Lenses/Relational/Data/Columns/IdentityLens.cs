using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Data.Model;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Data.Columns;

public abstract class IdentityLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData> 
    : DataColumnLens<TLeftDataColumn, TRightDataColumn, TLeftData, TRightData, IdentityLens>
    where TLeftDataColumn : notnull, DataColumn<TLeftData>
    where TRightDataColumn : notnull, DataColumn<TRightData>
    where TLeftData : notnull
    where TRightData : notnull
{
    protected IdentityLens(string targetColumnName, IdentityLens columnLens, ISymmetricLens<TLeftData, TRightData> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public override Func<TRightDataColumn, Option<TLeftDataColumn>, Result<TLeftDataColumn>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, Option.Some(target.Column))
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.Some(target.Data))
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn)),
            () => _columnLens.PutLeft(updatedSource.Column, Option.None<Column>())
                .Bind(leftColumn => _dataLens.PutLeft(updatedSource.Data, Option.None<TLeftData>())
                    .Map(leftData => new DataColumn<TLeftData>(leftColumn, leftData) as TLeftDataColumn))
        )!;

    public override Func<TLeftDataColumn, Option<TRightDataColumn>, Result<TRightDataColumn>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, Option.Some(target.Column))
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.Some(target.Data))
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn)),
            () => _columnLens.PutRight(updatedSource.Column, Option.None<Column>())
                .Bind(rightColumn => _dataLens.PutRight(updatedSource.Data, Option.None<TRightData>())
                    .Map(rightData => new DataColumn<TRightData>(rightColumn, rightData) as TRightDataColumn))
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


public sealed class IntegerIdentityLens : IdentityLens<IntegerDataColumn, IntegerDataColumn, int, int>
{
    private IntegerIdentityLens(string targetColumnName, IdentityLens columnLens, ISymmetricLens<int, int> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static IntegerIdentityLens Cons(string targetColumnName, IdentityLens columnLens, ISymmetricLens<int, int> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class StringIdentityLens : IdentityLens<StringDataColumn, StringDataColumn, string, string>
{
    private StringIdentityLens(string targetColumnName, IdentityLens columnLens, ISymmetricLens<string, string> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static StringIdentityLens Cons(string targetColumnName, IdentityLens columnLens, ISymmetricLens<string, string> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class BooleanIdentityLens : IdentityLens<BooleanDataColumn, BooleanDataColumn, bool, bool>
{
    private BooleanIdentityLens(string targetColumnName, IdentityLens columnLens, ISymmetricLens<bool, bool> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static BooleanIdentityLens Cons(string targetColumnName, IdentityLens columnLens, ISymmetricLens<bool, bool> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class DateTimeIdentityLens : IdentityLens<DateTimeDataColumn, DateTimeDataColumn, DateTime, DateTime>
{
    private DateTimeIdentityLens(string targetColumnName, IdentityLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static DateTimeIdentityLens Cons(string targetColumnName, IdentityLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}

public sealed class DecimalIdentityLens : IdentityLens<DecimalDataColumn, DecimalDataColumn, double, double>
{
    private DecimalIdentityLens(string targetColumnName, IdentityLens columnLens, ISymmetricLens<double, double> dataLens) : base(targetColumnName, columnLens, dataLens)
    {
    }

    public static DecimalIdentityLens Cons(string targetColumnName, IdentityLens columnLens, ISymmetricLens<double, double> dataLens) 
        => new(targetColumnName, columnLens, dataLens);
}