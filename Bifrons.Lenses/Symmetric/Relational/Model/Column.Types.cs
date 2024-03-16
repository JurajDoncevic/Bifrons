using Bifrons.Lenses.Symmetric.Relational.Model;
using Bifrons.Lenses.Symmetric.Relational.Columns;

namespace Bifrons.Lenses.Symmetric.Relational.Model;

public sealed class IntegerColumn : Column
{
    private IntegerColumn(string name) : base(name, DataTypes.INTEGER)
    {
    }

    public static IntegerColumn Cons(string name) => new IntegerColumn(name);
}

public sealed class DecimalColumn : Column
{
    private DecimalColumn(string name) : base(name, DataTypes.DECIMAL)
    {
    }

    public static DecimalColumn Cons(string name) => new DecimalColumn(name);
}

public sealed class StringColumn : Column
{
    private StringColumn(string name) : base(name, DataTypes.STRING)
    {
    }

    public static StringColumn Cons(string name) => new StringColumn(name);
}

public sealed class BooleanColumn : Column
{
    private BooleanColumn(string name) : base(name, DataTypes.BOOLEAN)
    {
    }

    public static BooleanColumn Cons(string name) => new BooleanColumn(name);
}

public sealed class DateTimeColumn : Column
{
    private DateTimeColumn(string name) : base(name, DataTypes.DATETIME)
    {
    }

    public static DateTimeColumn Cons(string name) => new DateTimeColumn(name);
}

/// <summary>
/// Column that represents a unit of data. This is useful for representing a column that is not included in a schema, but there is a need for a placeholder.
/// </summary>
public sealed class UnitColumn : Column
{
    private UnitColumn(string name) : base(name, DataTypes.UNIT)
    {
    }

    public static UnitColumn Cons(string name) => new UnitColumn(name);
}
