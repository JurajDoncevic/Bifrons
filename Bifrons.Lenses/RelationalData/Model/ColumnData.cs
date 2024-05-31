using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.RelationalData.Model;


public interface IColumnData<TData>
{
    Option<TData> Data { get; }
}

public abstract class ColumnData
{
    private Column _column;
    private object? _boxedData;

    public Column Column => _column;
    public object? BoxedData => _boxedData;
    public DataTypes DataType => _column.DataType;
    public string Name => _column.Name;
    public bool IsUnit => _column.DataType == DataTypes.UNIT;

    protected ColumnData(Column column, object? boxedData)
    {
        _column = column;
        _boxedData = boxedData;
    }

    public T GetData<T>() => (T)_boxedData!;
    public override bool Equals(object? obj)
        => obj is not null
            && obj is ColumnData
            && Equals((obj as ColumnData)!);

    private bool Equals(ColumnData other)
        => _column.Equals(other._column) && (_boxedData?.Equals(other._boxedData) ?? other._boxedData is null);

    public override int GetHashCode()
        => HashCode.Combine(_column, _boxedData);

    public static Result<ColumnData> Cons(Column column, object? boxedData = null)
     => Result.AsResult(
         () => column.DataType switch
         {
             DataTypes.STRING => StringColumnData.Cons((column as StringColumn)!, boxedData as string),
             DataTypes.INTEGER => IntegerColumnData.Cons((column as IntegerColumn)!, boxedData as int?),
             DataTypes.LONG => LongColumnData.Cons((column as LongColumn)!, boxedData as long?),
             DataTypes.DECIMAL => DecimalColumnData.Cons((column as DecimalColumn)!, boxedData as double?),
             DataTypes.BOOLEAN => BooleanColumnData.Cons((column as BooleanColumn)!, boxedData as bool?),
             DataTypes.DATETIME => DateTimeColumnData.Cons((column as DateTimeColumn)!, boxedData as DateTime?),
             DataTypes.UNIT => UnitColumnData.Cons((column as UnitColumn)!),
             _ => Result.Failure<ColumnData>($"Unsupported data type: {column.DataType}")
         });

    public static Result<TColumnData> Cons<TColumnData>(Column column, object? boxedData = null)
        where TColumnData : ColumnData
     => Result.AsResult(
         () => column.DataType switch
         {
             DataTypes.STRING => (StringColumnData.Cons((column as StringColumn)!, boxedData as string) as TColumnData)!,
             DataTypes.INTEGER => (IntegerColumnData.Cons((column as IntegerColumn)!, (int?)boxedData) as TColumnData)!,
             DataTypes.LONG => (LongColumnData.Cons((column as LongColumn)!, boxedData as long?) as TColumnData)!,
             DataTypes.DECIMAL => (DecimalColumnData.Cons((column as DecimalColumn)!, boxedData as double?) as TColumnData)!,
             DataTypes.BOOLEAN => (BooleanColumnData.Cons((column as BooleanColumn)!, boxedData as bool?) as TColumnData)!,
             DataTypes.DATETIME => (DateTimeColumnData.Cons((column as DateTimeColumn)!, boxedData as DateTime?) as TColumnData)!,
             DataTypes.UNIT => (UnitColumnData.Cons((column as UnitColumn)!) as TColumnData)!,
             _ => Result.Failure<TColumnData>($"Unsupported data type: {column.DataType}")
         });
}


public sealed class StringColumnData : ColumnData, IColumnData<string>
{
    public Option<string> Data => BoxedData as string ?? Option.None<string>();

    private StringColumnData(StringColumn column, string? data)
        : base(column, data)
    {
    }

    public static StringColumnData Cons(StringColumn column, string? data = null)
        => new(column, data);
}

public sealed class IntegerColumnData : ColumnData, IColumnData<int>
{
    public Option<int> Data => BoxedData as int? ?? Option.None<int>();

    private IntegerColumnData(IntegerColumn column, int? data)
        : base(column, data)
    {
    }

    public static IntegerColumnData Cons(IntegerColumn column, int? data = null)
        => new(column, data);
}

public sealed class LongColumnData : ColumnData, IColumnData<long>
{
    public Option<long> Data => BoxedData as long? ?? Option.None<long>();

    private LongColumnData(LongColumn column, long? data)
        : base(column, data)
    {
    }

    public static LongColumnData Cons(LongColumn column, long? data = null)
        => new(column, data);
}

public sealed class DecimalColumnData : ColumnData, IColumnData<double>
{
    public Option<double> Data => BoxedData as double? ?? Option.None<double>();

    private DecimalColumnData(DecimalColumn column, double? data)
        : base(column, data)
    {
    }

    public static DecimalColumnData Cons(DecimalColumn column, double? data = null)
        => new(column, data);
}

public sealed class BooleanColumnData : ColumnData, IColumnData<bool>
{
    public Option<bool> Data => BoxedData as bool? ?? Option.None<bool>();

    private BooleanColumnData(BooleanColumn column, bool? data)
        : base(column, data)
    {
    }

    public static BooleanColumnData Cons(BooleanColumn column, bool? data = null)
        => new(column, data);
}

public sealed class DateTimeColumnData : ColumnData, IColumnData<DateTime>
{
    public Option<DateTime> Data => BoxedData as DateTime? ?? Option.None<DateTime>();

    private DateTimeColumnData(DateTimeColumn column, DateTime? data)
        : base(column, data)
    {
    }

    public static DateTimeColumnData Cons(DateTimeColumn column, DateTime? data = null)
        => new(column, data);
}

public sealed class UnitColumnData : ColumnData, IColumnData<Unit>
{
    public Option<Unit> Data => Unit();

    private UnitColumnData(UnitColumn column)
        : base(column, null)
    {
    }

    public static UnitColumnData Cons()
        => new(UnitColumn.Cons());

    public static UnitColumnData Cons(string name = UnitColumn.UNIT_NAME)
        => new(UnitColumn.Cons(name));

    public static UnitColumnData Cons(UnitColumn column)
        => new(column);
}