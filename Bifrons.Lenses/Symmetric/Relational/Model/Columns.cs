using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses;

public sealed class IntegerColumn : Column
{
    public IntegerColumn(string name) : base(name, DataTypes.INT)
    {
    }
}

public sealed class DecimalColumn : Column
{
    public DecimalColumn(string name) : base(name, DataTypes.DECIMAL)
    {
    }
}

public sealed class StringColumn : Column
{
    public StringColumn(string name) : base(name, DataTypes.STRING)
    {
    }
}

public sealed class DateTimeColumn : Column
{
    public DateTimeColumn(string name) : base(name, DataTypes.DATETIME)
    {
    }
}
