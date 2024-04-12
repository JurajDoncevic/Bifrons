using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Strings;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class IdentityLens<TDataColumn, TData>
    : SymmetricDataColumnLens<TDataColumn, TDataColumn, TData>
    where TDataColumn : DataColumn, IDataColumn<TData>
{
    public IdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<TData, TData> dataLens)
        : base(columnLens, dataLens)
    {
    }


    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutLeft =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutLeft(l, r))
                                .Unfold()
                                .Bind(data => DataColumn.Cons(column, data.Cast<object?>()).Map(_ => _ as TDataColumn))
                                )!,
                () => CreateLeft(updatedSource)
                )!;

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutRight =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => _columnLens.PutRight(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutRight(l, r))
                                .Unfold()
                                .Bind(data => DataColumn.Cons(column, data.Cast<object?>()).Map(_ => _ as TDataColumn))
                                )!,
                () => CreateRight(updatedSource)
                )!;

    public override Func<TDataColumn, Result<TDataColumn>> CreateRight =>
        source => _columnLens.CreateRight(source.Column)
                    .Bind(column
                        => source.Data.Fold(
                                Enumerable.Empty<Result<TData>>(),
                                (data, res) => res.Append(_dataLens.CreateRight(data))
                                ).Unfold()
                                .Bind(data => DataColumn.Cons(column, data.Cast<object?>())
                                                .Map(_ => _ as TDataColumn))
                        )!;
    public override Func<TDataColumn, Result<TDataColumn>> CreateLeft =>
        source => _columnLens.CreateLeft(source.Column)
                    .Bind(column
                        => source.Data.Fold(
                                Enumerable.Empty<Result<TData>>(),
                                (data, res) => res.Append(_dataLens.CreateLeft(data))
                                ).Unfold()
                                .Bind(data => DataColumn.Cons(column, data.Cast<object?>())
                                                .Map(_ => _ as TDataColumn))
                        )!;
}

public sealed class IntegerIdentityLens : IdentityLens<IntegerDataColumn, int>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;

    public IntegerIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<int, int> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static IntegerIdentityLens Cons(SymmetricColumnLens columnLens, ISymmetricLens<int, int> dataLens)
        => new(columnLens, dataLens);
}

public sealed class StringIdentityLens : IdentityLens<StringDataColumn, string>
{
    public override DataTypes ForDataType => DataTypes.STRING;

    public StringIdentityLens(SymmetricColumnLens columnLens, SymmetricStringLens dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static StringIdentityLens Cons(SymmetricColumnLens columnLens, SymmetricStringLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class DateTimeIdentityLens : IdentityLens<DateTimeDataColumn, DateTime>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;

    public DateTimeIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static DateTimeIdentityLens Cons(SymmetricColumnLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens)
        => new(columnLens, dataLens);
}

public sealed class BooleanIdentityLens : IdentityLens<BooleanDataColumn, bool>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    public BooleanIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<bool, bool> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static BooleanIdentityLens Cons(SymmetricColumnLens columnLens, ISymmetricLens<bool, bool> dataLens)
        => new(columnLens, dataLens);
}

public sealed class DecimalIdentityLens : IdentityLens<DecimalDataColumn, double>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;

    public DecimalIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<double, double> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static DecimalIdentityLens Cons(SymmetricColumnLens columnLens, ISymmetricLens<double, double> dataLens)
        => new(columnLens, dataLens);
}

public sealed class UnitIdentityLens : IdentityLens<UnitDataColumn, Unit>
{
    public override DataTypes ForDataType => DataTypes.UNIT;

    public UnitIdentityLens(SymmetricColumnLens columnLens, ISymmetricLens<Unit, Unit> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static UnitIdentityLens Cons(SymmetricColumnLens columnLens, ISymmetricLens<Unit, Unit> dataLens)
        => new(columnLens, dataLens);
}