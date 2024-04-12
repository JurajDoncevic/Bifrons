using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.RelationalData.Model;

public interface IDataColumn<TData>
{
    IReadOnlyList<TData> Data { get; }
}

public abstract class DataColumn
{
    private Column _column;
    private List<object?> _boxedData;

    public Column Column => _column;
    public IReadOnlyList<object?> BoxedData => _boxedData;
    public DataTypes DataType => _column.DataType;
    public string Name => _column.Name;
    public bool IsUnit => _column.DataType == DataTypes.UNIT;

    protected DataColumn(Column column, IEnumerable<object?> boxedData)
    {
        _column = column;
        _boxedData = boxedData.ToList();
    }

    public static Result<DataColumn> Cons(Column column, IEnumerable<object?>? boxedData = null)
        => Result.AsResult(
            () => column.DataType switch
            {
                DataTypes.STRING => StringDataColumn.Cons((column as StringColumn)!, boxedData?.Cast<string?>()),
                DataTypes.INTEGER => IntegerDataColumn.Cons((column as IntegerColumn)!, boxedData?.Cast<int?>()),
                DataTypes.DECIMAL => DecimalDataColumn.Cons((column as DecimalColumn)!, boxedData?.Cast<double?>()),
                DataTypes.BOOLEAN => BooleanDataColumn.Cons((column as BooleanColumn)!, boxedData?.Cast<bool?>()),
                DataTypes.DATETIME => DateTimeDataColumn.Cons((column as DateTimeColumn)!, boxedData?.Cast<DateTime?>()),
                _ => Result.Failure<DataColumn>($"Unsupported data type: {column.DataType}")
            });
}

public class StringDataColumn : DataColumn, IDataColumn<string>
{
    public IReadOnlyList<string> Data => BoxedData.Cast<string>().ToList();

    private StringDataColumn(StringColumn column, IEnumerable<string?> data)
        : base(column, data.Cast<object?>())
    {
    }

    public static StringDataColumn Cons(StringColumn column, IEnumerable<string?>? data = null)
        => new StringDataColumn(column, data ?? []);
}

public class IntegerDataColumn : DataColumn, IDataColumn<int>
{
    public IReadOnlyList<int> Data => BoxedData.Cast<int>().ToList();

    private IntegerDataColumn(IntegerColumn column, IEnumerable<int?> data)
        : base(column, data.Cast<object?>())
    {
    }

    public static IntegerDataColumn Cons(IntegerColumn column, IEnumerable<int?>? data = null)
        => new IntegerDataColumn(column, data ?? []);
}

public class DecimalDataColumn : DataColumn, IDataColumn<double>
{
    public IReadOnlyList<double> Data => BoxedData.Cast<double>().ToList();

    private DecimalDataColumn(DecimalColumn column, IEnumerable<double?> data)
        : base(column, data.Cast<object?>())
    {
    }

    public static DecimalDataColumn Cons(DecimalColumn column, IEnumerable<double?>? data = null)
        => new DecimalDataColumn(column, data ?? []);
}

public class BooleanDataColumn : DataColumn, IDataColumn<bool>
{
    public IReadOnlyList<bool> Data => BoxedData.Cast<bool>().ToList();

    private BooleanDataColumn(BooleanColumn column, IEnumerable<bool?> data)
        : base(column, data.Cast<object?>())
    {
    }

    public static BooleanDataColumn Cons(BooleanColumn column, IEnumerable<bool?>? data = null)
        => new BooleanDataColumn(column, data ?? []);
}

public class DateTimeDataColumn : DataColumn, IDataColumn<DateTime>
{
    public IReadOnlyList<DateTime> Data => BoxedData.Cast<DateTime>().ToList();

    private DateTimeDataColumn(DateTimeColumn column, IEnumerable<DateTime?> data)
        : base(column, data.Cast<object?>())
    {
    }

    public static DateTimeDataColumn Cons(DateTimeColumn column, IEnumerable<DateTime?>? data = null)
        => new DateTimeDataColumn(column, data ?? []);
}

public class UnitDataColumn : DataColumn, IDataColumn<Unit>
{
    public IReadOnlyList<Unit> Data => BoxedData.Cast<Unit>().ToList();

    private UnitDataColumn(UnitColumn column)
        : base(column, [])
    {
    }

    public static UnitDataColumn Cons(UnitColumn column)
        => new UnitDataColumn(column);

    public static UnitDataColumn Cons(string name = UnitColumn.UNIT_NAME)
        => new UnitDataColumn(UnitColumn.Cons(name));

    public static UnitDataColumn Cons()
        => new UnitDataColumn(UnitColumn.Cons());
}