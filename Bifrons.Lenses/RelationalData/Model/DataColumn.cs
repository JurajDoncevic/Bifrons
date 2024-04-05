using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.RelationalData.Model;

public interface IDataColumn 
{
    string Name { get; }
    DataTypes DataType { get; }
    Type Type { get; }
    IReadOnlyList<object?> BoxedData { get; }

    public static IDataColumn Cons<TColumn, TData>(TColumn column, IEnumerable<TData> data)
        where TColumn : Column, IColumn<TData>
    {;
        return column.DataType switch 
        {
            DataTypes.INTEGER => IntegerDataColumn.Cons((column as IntegerColumn)!, data as IEnumerable<int>),
            DataTypes.STRING => StringDataColumn.Cons((column as StringColumn)!, data as IEnumerable<string>),
            DataTypes.BOOLEAN => BooleanDataColumn.Cons((column as BooleanColumn)!, data as IEnumerable<bool>),
            DataTypes.DECIMAL => DecimalDataColumn.Cons((column as DecimalColumn)!, data as IEnumerable<double>),
            DataTypes.DATETIME => DateTimeDataColumn.Cons((column as DateTimeColumn)!, data as IEnumerable<DateTime>),
            DataTypes.UNIT => UnitDataColumn.Cons((column as UnitColumn)!, data as IEnumerable<Unit>),
            _ => throw new ArgumentException($"Unsupported column data type: {column.DataType}")
        };
    }
 }

public abstract class DataColumn<TData, TColumn> : IDataColumn
    where TColumn : Column, IColumn<TData>
{
    private readonly List<TData> _data;
    private readonly TColumn _column;

    public IReadOnlyList<TData> Data => _data;
    public TColumn Column => _column;

    public string Name => _column.Name;
    public DataTypes DataType => typeof(TData).ToDataType();
    public Type Type => typeof(TData);
    public IReadOnlyList<object?> BoxedData => _data.Map(data => (object?)data).ToList();

    protected DataColumn(TColumn column, IEnumerable<TData> data)
    {
        _data = data.ToList();
        _column = column;
    }

    public bool Equals(IDataColumn? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Name == other.Name && DataType == other.DataType && Type == other.Type && BoxedData.SequenceEqual(other.BoxedData);
    }

    public override bool Equals(object? obj)
        => obj is IDataColumn other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Name);
        hash.Add(DataType);
        hash.Add(Type);
        hash.Add(BoxedData);
        return hash.ToHashCode();
    }
}

public class IntegerDataColumn : DataColumn<int, IntegerColumn>
{
    private IntegerDataColumn(IntegerColumn column, IEnumerable<int> data) : base(column, data)
    {
    }

    public static IntegerDataColumn Cons(IntegerColumn column, IEnumerable<int>? data = null)
        => new(column, data ?? []);

    public static IntegerDataColumn Cons(IntegerColumn column, params int[] data)
        => new(column, data ?? []);
}

public class StringDataColumn : DataColumn<string, StringColumn>
{
    private StringDataColumn(StringColumn column, IEnumerable<string> data) : base(column, data)
    {
    }

    public static StringDataColumn Cons(StringColumn column, IEnumerable<string>? data = null)
        => new(column, data ?? []);

    public static StringDataColumn Cons(StringColumn column, params string[] data)
        => new(column, data ?? []);
}

public class BooleanDataColumn : DataColumn<bool, BooleanColumn>
{
    private BooleanDataColumn(BooleanColumn column, IEnumerable<bool> data) : base(column, data)
    {
    }

    public static BooleanDataColumn Cons(BooleanColumn column, IEnumerable<bool>? data = null)
        => new(column, data ?? []);

    public static BooleanDataColumn Cons(BooleanColumn column, params bool[] data)
        => new(column, data ?? []);
}

public class DecimalDataColumn : DataColumn<double, DecimalColumn>
{
    private DecimalDataColumn(DecimalColumn column, IEnumerable<double> data) : base(column, data)
    {
    }

    public static DecimalDataColumn Cons(DecimalColumn column, IEnumerable<double>? data = null)
        => new(column, data ?? []);

    public static DecimalDataColumn Cons(DecimalColumn column, params double[] data)
        => new(column, data ?? []);
}

public class DateTimeDataColumn : DataColumn<DateTime, DateTimeColumn>
{
    private DateTimeDataColumn(DateTimeColumn column, IEnumerable<DateTime> data) : base(column, data)
    {
    }

    public static DateTimeDataColumn Cons(DateTimeColumn column, IEnumerable<DateTime>? data = null)
        => new(column, data ?? []);

    public static DateTimeDataColumn Cons(DateTimeColumn column, params DateTime[] data)
        => new(column, data ?? []);
}

public class UnitDataColumn : DataColumn<Unit, UnitColumn>
{
    private UnitDataColumn(UnitColumn column, IEnumerable<Unit> data) : base(column, data)
    {
    }

    public static UnitDataColumn Cons(UnitColumn column, IEnumerable<Unit>? data = null)
        => new(column, data ?? []);

    public static UnitDataColumn Cons(UnitColumn column, params Unit[] data)
        => new(column, data ?? []);
}


