using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class DeleteLens<TColumnData, TData>
    : SymmetricColumnDataLens<TColumnData, UnitColumnData, TData>
    where TColumnData : ColumnData
{
    protected readonly TData _defaultData;

    protected DeleteLens(Relational.Columns.DeleteLens columnLens, TData defaultData) : base(columnLens, Option.None<ISymmetricLens<TData, TData>>())
    {
        _defaultData = defaultData;
    }

    public override Func<UnitColumnData, Option<TColumnData>, Result<TColumnData>> PutLeft =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                        .Bind(column => ColumnData.Cons<TColumnData>(column, target.BoxedData))!,
            () => CreateLeft(updatedSource)
            );

    public override Func<TColumnData, Option<UnitColumnData>, Result<UnitColumnData>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, target.Column)
                        .Map(column => UnitColumnData.Cons((column as UnitColumn)!)),
            () => CreateRight(updatedSource)
            );

    public override Func<TColumnData, Result<UnitColumnData>> CreateRight => 
        source => _columnLens.CreateRight(source.Column)
                    .Map(column => UnitColumnData.Cons((column as UnitColumn)!));

    public override Func<UnitColumnData, Result<TColumnData>> CreateLeft => 
        source => _columnLens.CreateLeft(source.Column)
                    .Bind(column => ColumnData.Cons<TColumnData>(column, _defaultData))!;
}

public sealed class StringDeleteLens : DeleteLens<StringColumnData, string>
{
    public override DataTypes ForDataType => DataTypes.STRING;

    public StringDeleteLens(Relational.Columns.DeleteLens columnLens, string defaultData) : base(columnLens, defaultData)
    {
    }

    public static StringDeleteLens Cons(Relational.Columns.DeleteLens columnLens, string defaultData)
        => new(columnLens, defaultData);
}

public sealed class IntegerDeleteLens : DeleteLens<IntegerColumnData, int>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;

    public IntegerDeleteLens(Relational.Columns.DeleteLens columnLens, int defaultData) : base(columnLens, defaultData)
    {
    }

    public static IntegerDeleteLens Cons(Relational.Columns.DeleteLens columnLens, int defaultData)
        => new(columnLens, defaultData);
}

public sealed class DecimalDeleteLens : DeleteLens<DecimalColumnData, double>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;

    public DecimalDeleteLens(Relational.Columns.DeleteLens columnLens, double defaultData) : base(columnLens, defaultData)
    {
    }

    public static DecimalDeleteLens Cons(Relational.Columns.DeleteLens columnLens, double defaultData)
        => new(columnLens, defaultData);
}

public sealed class BooleanDeleteLens : DeleteLens<BooleanColumnData, bool>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    public BooleanDeleteLens(Relational.Columns.DeleteLens columnLens, bool defaultData) : base(columnLens, defaultData)
    {
    }

    public static BooleanDeleteLens Cons(Relational.Columns.DeleteLens columnLens, bool defaultData)
        => new(columnLens, defaultData);
}

public sealed class DateTimeDeleteLens : DeleteLens<DateTimeColumnData, DateTime>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;

    public DateTimeDeleteLens(Relational.Columns.DeleteLens columnLens, DateTime defaultData) : base(columnLens, defaultData)
    {
    }

    public static DateTimeDeleteLens Cons(Relational.Columns.DeleteLens columnLens, DateTime defaultData)
        => new(columnLens, defaultData);
}

public sealed class UnitDeleteLens : DeleteLens<UnitColumnData, Unit>
{
    public override DataTypes ForDataType => DataTypes.UNIT;

    public UnitDeleteLens(Relational.Columns.DeleteLens columnLens) : base(columnLens, Unit())
    {
    }

    public static UnitDeleteLens Cons(Relational.Columns.DeleteLens columnLens)
        => new(columnLens);
}