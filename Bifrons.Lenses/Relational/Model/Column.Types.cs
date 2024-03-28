namespace Bifrons.Lenses.Relational.Model;

public sealed class IntegerColumn : Column
{
    private IntegerColumn(string name) : base(name, DataTypes.INTEGER)
    {
    }

    public static IntegerColumn Cons(string name) => new(name);
}

public sealed class DecimalColumn : Column
{
    private DecimalColumn(string name) : base(name, DataTypes.DECIMAL)
    {
    }

    public static DecimalColumn Cons(string name) => new(name);
}

public sealed class StringColumn : Column
{
    private StringColumn(string name) : base(name, DataTypes.STRING)
    {
    }

    public static StringColumn Cons(string name) => new(name);
}

public sealed class BooleanColumn : Column
{
    private BooleanColumn(string name) : base(name, DataTypes.BOOLEAN)
    {
    }

    public static BooleanColumn Cons(string name) => new(name);
}

public sealed class DateTimeColumn : Column
{
    private DateTimeColumn(string name) : base(name, DataTypes.DATETIME)
    {
    }

    public static DateTimeColumn Cons(string name) => new(name);
}

/// <summary>
/// Column that represents a unit of data. This is useful for representing a column that is not included in a schema, but there is a need for a placeholder.
/// </summary>
public sealed class UnitColumn : Column
{
    private UnitColumn(string name) : base(name, DataTypes.UNIT)
    {
    }

    public static UnitColumn Cons(string name = "UNIT") => new(name);
}
