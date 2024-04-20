using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Columns;

public abstract class RenameLens<TColumnData, TData>
    : SymmetricColumnDataLens<TColumnData, TColumnData, TData>
    where TColumnData : ColumnData, IColumnData<TData>
{
    protected new readonly ISymmetricLens<TData, TData> _dataLens;

    protected RenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<TData, TData> dataLens)
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

public sealed class StringRenameLens : RenameLens<StringColumnData, string>
{
    public override DataTypes ForDataType => DataTypes.STRING;

    private StringRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<string, string> dataLens)
        : base(columnLens, dataLens)
    {   
    }

    public static StringRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<string, string> dataLens)
        => new StringRenameLens(columnLens, dataLens);
}

public sealed class IntegerRenameLens : RenameLens<IntegerColumnData, int>
{
    public override DataTypes ForDataType => DataTypes.INTEGER;

    private IntegerRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<int, int> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static IntegerRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<int, int> dataLens)
        => new IntegerRenameLens(columnLens, dataLens);
}

public sealed class LongRenameLens : RenameLens<LongColumnData, long>
{
    public override DataTypes ForDataType => DataTypes.LONG;

    private LongRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<long, long> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static LongRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<long, long> dataLens)
        => new LongRenameLens(columnLens, dataLens);
}

public sealed class BooleanRenameLens : RenameLens<BooleanColumnData, bool>
{
    public override DataTypes ForDataType => DataTypes.BOOLEAN;

    private BooleanRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<bool, bool> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static BooleanRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<bool, bool> dataLens)
        => new BooleanRenameLens(columnLens, dataLens);
}

public sealed class DateTimeRenameLens : RenameLens<DateTimeColumnData, DateTime>
{
    public override DataTypes ForDataType => DataTypes.DATETIME;

    private DateTimeRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static DateTimeRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<DateTime, DateTime> dataLens)
        => new DateTimeRenameLens(columnLens, dataLens);
}

public sealed class DecimalRenameLens : RenameLens<DecimalColumnData, double>
{
    public override DataTypes ForDataType => DataTypes.DECIMAL;

    private DecimalRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<double, double> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static DecimalRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<double, double> dataLens)
        => new DecimalRenameLens(columnLens, dataLens);
}

public sealed class UnitRenameLens : RenameLens<UnitColumnData, Unit>
{
    public override DataTypes ForDataType => DataTypes.UNIT;

    private UnitRenameLens(Relational.Columns.RenameLens columnLens, ISymmetricLens<Unit, Unit> dataLens)
        : base(columnLens, dataLens)
    {
    }

    public static UnitRenameLens Cons(Relational.Columns.RenameLens columnLens, ISymmetricLens<Unit, Unit> dataLens)
        => new UnitRenameLens(columnLens, dataLens);
}