
using Bifrons.Lenses.Symmetric.DateTimes;
using Bifrons.Lenses.Symmetric.Decimals;
using Bifrons.Lenses.Symmetric.Integers;
using Bifrons.Lenses.Symmetric.Strings;

namespace Bifrons.Lenses.Symmetric.Relational;

public class ColumnLens<T>
    : BaseSymmetricLens<T, T>
    where T : notnull
{
    private readonly string _columnName;

    private readonly BaseSymmetricLens<T, T> _dataLens;

    public override Func<T, Option<T>, Result<T>> PutLeft => throw new NotImplementedException();

    public override Func<T, Option<T>, Result<T>> PutRight => throw new NotImplementedException();

    public override Func<T, Result<T>> CreateRight => throw new NotImplementedException();

    public override Func<T, Result<T>> CreateLeft => throw new NotImplementedException();

    private ColumnLens(string columnName, BaseSymmetricLens<T, T> dataLens)
    {
        _columnName = columnName;
        _dataLens = dataLens;
    }



    public static ColumnLens<T> Cons(string columnName, BaseSymmetricLens<T, T> dataLens)
        => new(columnName, dataLens);

    public static ColumnLens<string> Cons(string columnName, SymmetricStringLens dataLens)
        => new(columnName, dataLens);

    public static ColumnLens<string> ConsOnString(string columnName)
        => new(columnName, Strings.IdentityLens.Cons(".*"));
    
    public static ColumnLens<int> Cons(string columnName, SymmetricIntegerLens dataLens)
        => new(columnName, dataLens);

    public static ColumnLens<int> ConsOnInteger(string columnName)
        => new(columnName, Integers.IdentityLens.Cons());

    public static ColumnLens<double> Cons(string columnName, SymmetricDecimalLens dataLens)
        => new(columnName, dataLens);

    public static ColumnLens<double> ConsOnDecimal(string columnName)
        => new(columnName, Decimals.IdentityLens.Cons());

    public static ColumnLens<DateTime> Cons(string columnName, SymmetricDateTimeLens dataLens)
        => new(columnName, dataLens);

    public static ColumnLens<DateTime> ConsOnDateTime(string columnName)
        => new(columnName, DateTimes.IdentityLens.Cons());
}