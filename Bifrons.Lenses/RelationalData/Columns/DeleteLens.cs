﻿using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class DeleteLens<TDataColumn, TData, TColumn>
    : SymmetricDataColumnLens<TDataColumn, TData, TColumn, TDataColumn, TData, TColumn>
    where TDataColumn : DataColumn<TData, TColumn>
    where TColumn : Column, IColumn<TData>
{
    protected DeleteLens(DeleteLens columnLens, ISymmetricLens<TData, TData> dataLens) : base(columnLens, dataLens)
    {
    }

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutLeft => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutLeft(l, r))
                                .Unfold()
                                .Map(data => IDataColumn.Cons((column as TColumn)!, data) as TDataColumn)
                                )!,
                () => CreateLeft(updatedSource)
                )!;

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutRight => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => _columnLens.PutRight(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutRight(l, r))
                                .Unfold()
                                .Map(data => UnitDataColumn.Cons() as TDataColumn)
                                )!,
                () => CreateRight(updatedSource)
                )!;

    public override Func<TDataColumn, Result<TDataColumn>> CreateRight =>
        source => _columnLens.CreateRight(source.Column)
            .Bind(column => source.Data.Fold(
                Enumerable.Empty<Result<TData>>(),
                (data, res) => res.Append(_dataLens.CreateRight(data))
            ).Unfold()
            .Map(data => UnitDataColumn.Cons() as TDataColumn))!;

    public override Func<TDataColumn, Result<TDataColumn>> CreateLeft => 
        source => _columnLens.CreateLeft(source.Column)
            .Bind(column => source.Data.Fold(
                Enumerable.Empty<Result<TData>>(),
                (data, res) => res.Append(_dataLens.CreateLeft(data))
            ).Unfold()
            .Map(data => UnitDataColumn.Cons() as TDataColumn))!;

}


public sealed class IntegerDeleteLens : DeleteLens<IntegerDataColumn, int, IntegerColumn>
{
    private IntegerDeleteLens(DeleteLens columnLens, Integers.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static IntegerDeleteLens Cons(DeleteLens columnLens, Integers.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class StringDeleteLens : DeleteLens<StringDataColumn, string, StringColumn>
{
    private StringDeleteLens(DeleteLens columnLens, Strings.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static StringDeleteLens Cons(DeleteLens columnLens, Strings.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class DateTimeDeleteLens : DeleteLens<DateTimeDataColumn, DateTime, DateTimeColumn>
{
    private DateTimeDeleteLens(DeleteLens columnLens, DateTimes.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static DateTimeDeleteLens Cons(DeleteLens columnLens, DateTimes.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class DecimalDeleteLens : DeleteLens<DecimalDataColumn, double, DecimalColumn>
{
    private DecimalDeleteLens(DeleteLens columnLens, Decimals.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static DecimalDeleteLens Cons(DeleteLens columnLens, Decimals.DeleteLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class BooleanDeleteLens : DeleteLens<BooleanDataColumn, bool, BooleanColumn>
{
    private BooleanDeleteLens(DeleteLens columnLens, Booleans.DeleteLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static BooleanDeleteLens Cons(DeleteLens columnLens, Booleans.DeleteLens dataLens)
        => new(columnLens, dataLens);
}