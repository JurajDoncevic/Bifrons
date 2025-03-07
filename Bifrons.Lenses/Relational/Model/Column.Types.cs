﻿namespace Bifrons.Lenses.Relational.Model;

/// <summary>
/// Column type tag interface.
/// </summary>
public interface IColumn<TData> { }

/// <summary>
/// Represents an integer column in a table.
/// </summary>
public sealed class IntegerColumn : Column, IColumn<int>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private IntegerColumn(string name) : base(name, DataTypes.INTEGER)
    {
    }

    /// <summary>
    /// Constructs a new integer column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static IntegerColumn Cons(string name) => new(name);
}

public sealed class LongColumn : Column, IColumn<long>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private LongColumn(string name) : base(name, DataTypes.LONG)
    {
    }

    /// <summary>
    /// Constructs a new long column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static LongColumn Cons(string name) => new(name);
}

/// <summary>
/// Represents a decimal column in a table.
/// </summary>
public sealed class DecimalColumn : Column, IColumn<double>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private DecimalColumn(string name) : base(name, DataTypes.DECIMAL)
    {
    }

    /// <summary>
    /// Constructs a new decimal column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static DecimalColumn Cons(string name) => new(name);
}

/// <summary>
/// Represents a string column in a table.
/// </summary>
public sealed class StringColumn : Column, IColumn<string>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private StringColumn(string name) : base(name, DataTypes.STRING)
    {
    }

    /// <summary>
    /// Constructs a new string column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static StringColumn Cons(string name) => new(name);
}

/// <summary>
/// Represents a boolean column in a table.
/// </summary>
public sealed class BooleanColumn : Column, IColumn<bool>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private BooleanColumn(string name) : base(name, DataTypes.BOOLEAN)
    {
    }

    /// <summary>
    /// Constructs a new boolean column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static BooleanColumn Cons(string name) => new(name);
}

/// <summary>
/// Represents a date time column in a table.
/// </summary>
public sealed class DateTimeColumn : Column, IColumn<DateTime>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private DateTimeColumn(string name) : base(name, DataTypes.DATETIME)
    {
    }

    /// <summary>
    /// Constructs a new date time column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static DateTimeColumn Cons(string name) => new(name);
}

/// <summary>
/// Column that represents a unit of data. This is useful for representing a column that is not included in a schema, but when there is a need for a placeholder.
/// </summary>
public sealed class UnitColumn : Column, IColumn<Unit>
{
    public const string UNIT_NAME = "";

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Column name</param>
    private UnitColumn(string name) : base(name, DataTypes.UNIT)
    {
    }

    /// <summary>
    /// Constructs a new unit column.
    /// </summary>
    /// <param name="name">Column name</param>
    public static UnitColumn Cons(string name = UnitColumn.UNIT_NAME) => new(name);
}
