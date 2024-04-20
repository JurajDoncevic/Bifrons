using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;
using Bifrons.Lenses.Strings;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class IdentityLens<TColumnData, TData>
    : SymmetricColumnDataLens<TColumnData, TColumnData, TData>
    where TColumnData : ColumnData, IColumnData<TData>
{
    private new readonly ISymmetricLens<TData, TData> _dataLens;

    protected IdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<TData, TData> dataLens)
        : base(columnLens, Option.Some(dataLens))
    {
        _dataLens = dataLens;
    }

    public override Func<TColumnData, Option<TColumnData>, Result<TColumnData>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, target.Column)
                        .Bind(column => updatedSource.Data.Match(
                            sourceData => _dataLens.PutRight(sourceData, target.Data).Bind(data => ColumnData.Cons<TColumnData>(column, data))!,
                            () => ColumnData.Cons(column) as TColumnData
                            )
                        )!,
            () => CreateRight(updatedSource)
            );
    public override Func<TColumnData, Option<TColumnData>, Result<TColumnData>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                        .Bind(column => updatedSource.Data.Match(
                            sourceData => _dataLens.PutLeft(sourceData, target.Data).Bind(data => ColumnData.Cons<TColumnData>(column, data))!,
                            () => ColumnData.Cons<TColumnData>(column)
                            )
                        )!,
            () => CreateLeft(updatedSource)
            );
    public override Func<TColumnData, Result<TColumnData>> CreateRight => 
        source => _columnLens.CreateRight(source.Column)
                    .Bind(column => source.Data.Match(
                        sourceData => _dataLens.CreateRight(sourceData).Bind(data => ColumnData.Cons<TColumnData>(column, data))!,
                        () => ColumnData.Cons<TColumnData>(column)
                        )
                    )!;
    public override Func<TColumnData, Result<TColumnData>> CreateLeft => 
        source => _columnLens.CreateLeft(source.Column)
                    .Bind(column => source.Data.Match(
                        sourceData => _dataLens.CreateLeft(sourceData).Bind(data => ColumnData.Cons<TColumnData>(column, data))!,
                        () => ColumnData.Cons<TColumnData>(column)
                        )
                    )!;
}

public sealed class IntegerIdentityLens : IdentityLens<IntegerColumnData, int>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;

    private IntegerIdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<int, int> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static IntegerIdentityLens Cons(Relational.Columns.IdentityLens columnLens, ISymmetricLens<int, int> dataLens)
        => new(columnLens, dataLens);
}

public sealed class LongIdentityLens : IdentityLens<LongColumnData, long>
{
    public override DataTypes ForDataType => DataTypes.LONG;

    private LongIdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<long, long> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static LongIdentityLens Cons(Relational.Columns.IdentityLens columnLens, ISymmetricLens<long, long> dataLens)
        => new(columnLens, dataLens);
}

public sealed class StringIdentityLens : IdentityLens<StringColumnData, string>
{
    public override DataTypes ForDataType => DataTypes.STRING;

    private StringIdentityLens(Relational.Columns.IdentityLens columnLens, SymmetricStringLens dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static StringIdentityLens Cons(Relational.Columns.IdentityLens columnLens, SymmetricStringLens dataLens)
        => new(columnLens, dataLens);
}

public sealed class DateTimeIdentityLens : IdentityLens<DateTimeColumnData, DateTime>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;

    private DateTimeIdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static DateTimeIdentityLens Cons(Relational.Columns.IdentityLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens)
        => new(columnLens, dataLens);
}

public sealed class BooleanIdentityLens : IdentityLens<BooleanColumnData, bool>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    private BooleanIdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<bool, bool> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static BooleanIdentityLens Cons(Relational.Columns.IdentityLens columnLens, ISymmetricLens<bool, bool> dataLens)
        => new(columnLens, dataLens);
}

public sealed class DecimalIdentityLens : IdentityLens<DecimalColumnData, double>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;

    private DecimalIdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<double, double> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static DecimalIdentityLens Cons(Relational.Columns.IdentityLens columnLens, ISymmetricLens<double, double> dataLens)
        => new(columnLens, dataLens);
}

public sealed class UnitIdentityLens : IdentityLens<UnitColumnData, Unit>
{
    public override DataTypes ForDataType => DataTypes.UNIT;

    private UnitIdentityLens(Relational.Columns.IdentityLens columnLens, ISymmetricLens<Unit, Unit> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static UnitIdentityLens Cons(Relational.Columns.IdentityLens columnLens, ISymmetricLens<Unit, Unit> dataLens)
        => new(columnLens, dataLens);
}