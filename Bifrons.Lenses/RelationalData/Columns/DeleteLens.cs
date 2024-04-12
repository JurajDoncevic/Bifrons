using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class DeleteLens<TDataColumn, TData>
    : SymmetricDataColumnLens<TDataColumn, UnitDataColumn, TData>
    where TDataColumn : DataColumn, IDataColumn<TData>
{
    protected DeleteLens(DeleteLens columnLens, ISymmetricLens<TData, TData> dataLens) : base(columnLens, dataLens)
    {
    }

    public override Func<UnitDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutLeft =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutLeft(l.Covalesce<TData>(), r))
                                .Unfold()
                                .Bind(data => DataColumn.Cons(column, data.Cast<object?>()).Map(_ => _ as TDataColumn))
                                )!,
                () => CreateLeft(updatedSource)
                )!;

    public override Func<TDataColumn, Option<UnitDataColumn>, Result<UnitDataColumn>> PutRight =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => _columnLens.PutRight(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutRight(l, r.Covalesce<TData>()))
                                .Unfold()
                                .Map(data => UnitDataColumn.Cons(column.Name))
                                )!,
                () => CreateRight(updatedSource)
                )!;

    public override Func<TDataColumn, Result<UnitDataColumn>> CreateRight =>
        source => _columnLens.CreateRight(source.Column)
            .Bind(column => source.Data.Fold(
                Enumerable.Empty<Result<TData>>(),
                (data, res) => res.Append(_dataLens.CreateRight(data))
            ).Unfold()
            .Map(data => UnitDataColumn.Cons()))!;


    public override Func<UnitDataColumn, Result<TDataColumn>> CreateLeft =>
        source => _columnLens.CreateLeft(source.Column)
            .Bind(column => source.Data.Fold(
                Enumerable.Empty<Result<TData>>(),
                (data, res) => res.Append(_dataLens.CreateLeft(data.Covalesce<TData>()))
            ).Unfold()
            .Map(data => UnitDataColumn.Cons() as TDataColumn))!;

}


public sealed class IntegerDeleteLens : DeleteLens<IntegerDataColumn, int>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;
    private IntegerDeleteLens(DeleteLens columnLens, Integers.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static IntegerDeleteLens Cons(DeleteLens columnLens, Integers.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class StringDeleteLens : DeleteLens<StringDataColumn, string>
{
    public override DataTypes ForDataType => DataTypes.STRING;
    private StringDeleteLens(DeleteLens columnLens, Strings.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static StringDeleteLens Cons(DeleteLens columnLens, Strings.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class DateTimeDeleteLens : DeleteLens<DateTimeDataColumn, DateTime>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;
    private DateTimeDeleteLens(DeleteLens columnLens, DateTimes.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static DateTimeDeleteLens Cons(DeleteLens columnLens, DateTimes.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class DecimalDeleteLens : DeleteLens<DecimalDataColumn, double>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;
    private DecimalDeleteLens(DeleteLens columnLens, Decimals.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static DecimalDeleteLens Cons(DeleteLens columnLens, Decimals.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class BooleanDeleteLens : DeleteLens<BooleanDataColumn, bool>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    private BooleanDeleteLens(DeleteLens columnLens, Booleans.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static BooleanDeleteLens Cons(DeleteLens columnLens, Booleans.DeleteLens dataLens)
        => new(columnLens, dataLens);
}