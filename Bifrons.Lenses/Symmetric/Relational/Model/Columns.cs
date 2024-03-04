using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses;

public sealed class IntegerColumn : Column<int>
{
    public IntegerColumn(string name, int data) : base(name, DataTypes.INT, data)
    {
    }
}

public sealed class DecimalColumn : Column<decimal>
{
    public DecimalColumn(string name, decimal data) : base(name, DataTypes.DECIMAL, data)
    {
    }
}

public sealed class StringColumn : Column<string>
{
    public StringColumn(string name, string data) : base(name, DataTypes.STRING, data)
    {
    }
}

public sealed class DateTimeColumn : Column<DateTime>
{
    public DateTimeColumn(string name, DateTime data) : base(name, DataTypes.DATETIME, data)
    {
    }
}
