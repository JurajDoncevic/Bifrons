using Bifrons.Lenses.Symmetric.DateTimes;
using Bifrons.Lenses.Symmetric.Decimals;
using Bifrons.Lenses.Symmetric.Integers;
using Bifrons.Lenses.Symmetric.Relational.Model;
using Bifrons.Lenses.Symmetric.Strings;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class DeleteLens<TSrc, TDest> : SymmetricColumnLens<TSrc, TDest>
{
    private readonly string _columnName;

    private readonly BaseSymmetricLens<TSrc, TDest> _dataLens;

    public override Func<Column<TDest>, Option<Column<TSrc>>, Result<Column<TSrc>>> PutLeft => throw new NotImplementedException();

    public override Func<Column<TSrc>, Option<Column<TDest>>, Result<Column<TDest>>> PutRight => throw new NotImplementedException();

    public override Func<Column<TSrc>, Result<Column<TDest>>> CreateRight => throw new NotImplementedException();

    public override Func<Column<TDest>, Result<Column<TSrc>>> CreateLeft => throw new NotImplementedException();

    private DeleteLens(string columnName, BaseSymmetricLens<TSrc, TDest> dataLens)
    {
        _columnName = columnName;
        _dataLens = dataLens;
    }

    public static DeleteLens<TSrc, TDest> Cons(string columnName, BaseSymmetricLens<TSrc, TDest> dataLens)
        => new(columnName, dataLens);

    public static DeleteLens<string, string> Cons(string columnName, SymmetricStringLens dataLens)
        => new(columnName, dataLens);
    
    public static DeleteLens<int, int> Cons(string columnName, SymmetricIntegerLens dataLens)
        => new(columnName, dataLens);

    public static DeleteLens<double, double> Cons(string columnName, SymmetricDecimalLens dataLens)
        => new(columnName, dataLens);

    public static DeleteLens<DateTime, DateTime> Cons(string columnName, SymmetricDateTimeLens dataLens)
        => new(columnName, dataLens);
    

}

