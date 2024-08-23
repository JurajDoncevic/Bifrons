using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public interface IInsertLens : ISymmetricColumnDataLens
{
    new string MatchesColumnNameLeft { get; }
    new string MatchesColumnNameRight { get; }
    new bool MatchesLeft { get; }
    new bool MatchesRight { get; }
    new DataTypes ForDataType { get; }
    new SymmetricColumnLens ColumnLens { get; }

    new Func<ColumnData, Option<ColumnData>, Result<ColumnData>> PutLeft { get; }
    new Func<UnitColumnData, Option<ColumnData>, Result<ColumnData>> PutRight { get; }
    new Func<UnitColumnData, Result<ColumnData>> CreateRight { get; }
    new Func<ColumnData, Result<ColumnData>> CreateLeft { get; }
}

public abstract class InsertLens<TColumnData, TData>
    : SymmetricColumnDataLens<UnitColumnData, TColumnData, TData>,
      IInsertLens
    where TColumnData : ColumnData
{
    private readonly TData _defaultData;
    protected InsertLens(Relational.Columns.InsertLens columnLens, TData defaultData)
        : base(columnLens, Option.None<ISymmetricLens<TData, TData>>())
    {
        _defaultData = defaultData;
    }

    public override Func<UnitColumnData, Option<TColumnData>, Result<TColumnData>> PutRight =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutRight(updatedSource.Column, target.Column)
                        .Bind(column => ColumnData.Cons<TColumnData>(column, target.BoxedData)),
            () => CreateRight(updatedSource)
            );

    public override Func<TColumnData, Option<UnitColumnData>, Result<UnitColumnData>> PutLeft =>
        (updatedSource, originalTarget) => originalTarget.Match(
            target => _columnLens.PutLeft(updatedSource.Column, target.Column)
                        .Bind(column => ColumnData.Cons<UnitColumnData>(column)),
            () => CreateLeft(updatedSource)
            );

    public override Func<UnitColumnData, Result<TColumnData>> CreateRight =>
        source => _columnLens.CreateRight(source.Column)
                    .Bind(column => ColumnData.Cons<TColumnData>(column, _defaultData));

    public override Func<TColumnData, Result<UnitColumnData>> CreateLeft =>
        source => _columnLens.CreateLeft(source.Column)
                    .Bind(column => ColumnData.Cons<UnitColumnData>(column));

    Func<ColumnData, Option<ColumnData>, Result<ColumnData>> IInsertLens.PutLeft =>
        (updatedSource, originalTarget) => PutLeft((TColumnData)updatedSource, originalTarget.Map(_ => (UnitColumnData)_)).Map(_ => (ColumnData)_);

    Func<UnitColumnData, Option<ColumnData>, Result<ColumnData>> IInsertLens.PutRight =>
        (updatedSource, originalTarget) => PutRight((UnitColumnData)updatedSource, originalTarget.Map(_ => (TColumnData)_)).Map(_ => (ColumnData)_);

    Func<UnitColumnData, Result<ColumnData>> IInsertLens.CreateRight =>
        source => CreateRight((UnitColumnData)source).Map(_ => (ColumnData)_);

    Func<ColumnData, Result<ColumnData>> IInsertLens.CreateLeft =>
        source => CreateLeft((TColumnData)source).Map(_ => (ColumnData)_);
}


public sealed class StringInsertLens : InsertLens<StringColumnData, string>
{
    public override DataTypes ForDataType => DataTypes.STRING;

    private StringInsertLens(Relational.Columns.InsertLens columnLens, string defaultData) : base(columnLens, defaultData)
    {
    }

    public static StringInsertLens Cons(Relational.Columns.InsertLens columnLens, string defaultData)
        => new(columnLens, defaultData);
}

public sealed class IntegerInsertLens : InsertLens<IntegerColumnData, int>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;

    private IntegerInsertLens(Relational.Columns.InsertLens columnLens, int defaultData) : base(columnLens, defaultData)
    {
    }

    public static IntegerInsertLens Cons(Relational.Columns.InsertLens columnLens, int defaultData)
        => new(columnLens, defaultData);
}

public sealed class LongInsertLens : InsertLens<LongColumnData, long>
{
    public override DataTypes ForDataType => DataTypes.LONG;

    private LongInsertLens(Relational.Columns.InsertLens columnLens, long defaultData) : base(columnLens, defaultData)
    {
    }

    public static LongInsertLens Cons(Relational.Columns.InsertLens columnLens, long defaultData)
        => new(columnLens, defaultData);
}

public sealed class DecimalInsertLens : InsertLens<DecimalColumnData, double>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;

    private DecimalInsertLens(Relational.Columns.InsertLens columnLens, double defaultData) : base(columnLens, defaultData)
    {
    }

    public static DecimalInsertLens Cons(Relational.Columns.InsertLens columnLens, double defaultData)
        => new(columnLens, defaultData);
}

public sealed class BooleanInsertLens : InsertLens<BooleanColumnData, bool>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    private BooleanInsertLens(Relational.Columns.InsertLens columnLens, bool defaultData) : base(columnLens, defaultData)
    {
    }

    public static BooleanInsertLens Cons(Relational.Columns.InsertLens columnLens, bool defaultData)
        => new(columnLens, defaultData);
}

public sealed class DateTimeInsertLens : InsertLens<DateTimeColumnData, DateTime>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;

    private DateTimeInsertLens(Relational.Columns.InsertLens columnLens, DateTime defaultData) : base(columnLens, defaultData)
    {
    }

    public static DateTimeInsertLens Cons(Relational.Columns.InsertLens columnLens, DateTime defaultData)
        => new(columnLens, defaultData);
}

public sealed class UnitInsertLens : InsertLens<UnitColumnData, Unit>
{
    public override DataTypes ForDataType => DataTypes.UNIT;

    private UnitInsertLens(Relational.Columns.InsertLens columnLens) : base(columnLens, Unit())
    {
    }

    public static UnitInsertLens Cons(Relational.Columns.InsertLens columnLens)
        => new(columnLens);
}