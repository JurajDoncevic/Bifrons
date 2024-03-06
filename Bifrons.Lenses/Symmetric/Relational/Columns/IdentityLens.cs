
using Bifrons.Lenses.Symmetric.DateTimes;
using Bifrons.Lenses.Symmetric.Decimals;
using Bifrons.Lenses.Symmetric.Integers;
using Bifrons.Lenses.Symmetric.Relational.Model;
using Bifrons.Lenses.Symmetric.Strings;

namespace Bifrons.Lenses.Symmetric.Relational.Columns
;

public sealed class IdentityLens<TSrc, TTgt> : SymmetricColumnLens
{
    private readonly string _columnName;

    private readonly BaseSymmetricLens<TSrc, TTgt> _dataLens;

    private IdentityLens(string columnName, BaseSymmetricLens<TSrc, TTgt> dataLens)
    {
        _columnName = columnName;
        _dataLens = dataLens;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft => throw new NotImplementedException();

    public override Func<Column, Option<Column>, Result<Column>> PutRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateLeft => throw new NotImplementedException();

    public static IdentityLens<TSrc, TTgt> Cons(string columnName, BaseSymmetricLens<TSrc, TTgt> dataLens)
        => new(columnName, dataLens);

    public static IdentityLens<string, string> Cons(string columnName, SymmetricStringLens dataLens)
        => new(columnName, dataLens);

    public static IdentityLens<int, int> Cons(string columnName, SymmetricIntegerLens dataLens)
        => new(columnName, dataLens);

    public static IdentityLens<double, double> Cons(string columnName, SymmetricDecimalLens dataLens)
        => new(columnName, dataLens);

    public static IdentityLens<DateTime, DateTime> Cons(string columnName, SymmetricDateTimeLens dataLens)
        => new(columnName, dataLens);
}