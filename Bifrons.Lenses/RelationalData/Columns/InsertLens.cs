using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class InsertLens<TDataColumn, TData, TColumn>
    : SymmetricDataColumnLens<TDataColumn, TData, TColumn, TDataColumn, TData, TColumn>
    where TDataColumn : DataColumn<TData, TColumn>, IDataColumn
    where TColumn : Column, IColumn<TData>
{
    protected InsertLens(InsertLens columnLens, ISymmetricLens<TData, TData> dataLens) : base(columnLens, dataLens)
    {
    }

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutLeft =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutLeft(l, r))
                                .Unfold()
                                
                                .Map(data => UnitDataColumn.Cons() as TDataColumn)
                                )!,
                () => CreateLeft(updatedSource)
                )!;

    public override Func<TDataColumn, Option<TDataColumn>, Result<TDataColumn>> PutRight =>
        (updatedSource, originalTarget)
            => originalTarget.Match(
                target => _columnLens.PutRight(updatedSource.Column, target.Column)
                            .Bind(column => updatedSource.Data.Zip(target.Data, (l, r) => _dataLens.PutRight(l, r))
                                .Unfold()
                                .Map(data => IDataColumn.Cons((column as TColumn)!, data) as TDataColumn)
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


public sealed class IntegerInsertLens : InsertLens<IntegerDataColumn, int, IntegerColumn>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;

    private IntegerInsertLens(InsertLens columnLens, Integers.InsertLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static IntegerInsertLens Cons(InsertLens columnLens, Integers.InsertLens dataLens)
        => new (columnLens, dataLens);
}

public sealed class StringInsertLens : InsertLens<StringDataColumn, string, StringColumn>
{
    public override DataTypes ForDataType => DataTypes.STRING;

    private StringInsertLens(InsertLens columnLens, Strings.InsertLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static StringInsertLens Cons(InsertLens columnLens, Strings.InsertLens dataLens)
        => new (columnLens, dataLens);
}

public sealed class DateTimeInsertLens : InsertLens<DateTimeDataColumn, DateTime, DateTimeColumn>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;

    private DateTimeInsertLens(InsertLens columnLens, DateTimes.InsertLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static DateTimeInsertLens Cons(InsertLens columnLens, DateTimes.InsertLens dataLens)
        => new (columnLens, dataLens);
}

public sealed class BooleanInsertLens : InsertLens<BooleanDataColumn, bool, BooleanColumn>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    private BooleanInsertLens(InsertLens columnLens, Booleans.InsertLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static BooleanInsertLens Cons(InsertLens columnLens, Booleans.InsertLens dataLens)
        => new (columnLens, dataLens);
}

public sealed class DecimalInsertLens : InsertLens<DecimalDataColumn, double, DecimalColumn>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;

    private DecimalInsertLens(InsertLens columnLens, Decimals.InsertLens dataLens) : base(columnLens, dataLens)
    {
    }

    public static DecimalInsertLens Cons(InsertLens columnLens, Decimals.InsertLens dataLens)
        => new (columnLens, dataLens);
}