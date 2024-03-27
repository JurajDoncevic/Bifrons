using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Data.Model;

public interface IDataColumn 
{
    Column Column { get; }
    DataTypes DataType { get; }
}

public class DataColumn<TData> : IDataColumn
{
    private readonly Column _column;
    private readonly TData _data;

    public Column Column => _column;
    public TData Data => _data;

    public DataTypes DataType => typeof(TData).ToDataType();

    public DataColumn(Column column, TData data)
    {
        _column = column;
        _data = data;
    }

}

public sealed class IntegerDataColumn : DataColumn<int>
{
    private IntegerDataColumn(IntegerColumn column, int data) : base(column, data)
    {
    }

    public static IntegerDataColumn Cons(IntegerColumn column, int data) => new(column, data);
}

public class StringDataColumn : DataColumn<string>
{
    private StringDataColumn(StringColumn column, string data) : base(column, data)
    {
    }

    public static StringDataColumn Cons(StringColumn column, string data) => new(column, data);
}

public sealed class BooleanDataColumn : DataColumn<bool>
{
    private BooleanDataColumn(BooleanColumn column, bool data) : base(column, data)
    {
    }

    public static BooleanDataColumn Cons(BooleanColumn column, bool data) => new(column, data);
}

public sealed class DateTimeDataColumn : DataColumn<DateTime>
{
    private  DateTimeDataColumn(DateTimeColumn column, DateTime data) : base(column, data)
    {
    }

    public static DateTimeDataColumn Cons(DateTimeColumn column, DateTime data) => new(column, data);
}

public sealed class DecimalDataColumn : DataColumn<double>
{
    private DecimalDataColumn(DecimalColumn column, double data) : base(column, data)
    {
    }

    public static DecimalDataColumn Cons(DecimalColumn column, double data) => new(column, data);
}
