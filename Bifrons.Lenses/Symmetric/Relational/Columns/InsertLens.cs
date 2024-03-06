using Bifrons.Lenses.Symmetric.DateTimes;
using Bifrons.Lenses.Symmetric.Decimals;
using Bifrons.Lenses.Symmetric.Integers;
using Bifrons.Lenses.Symmetric.Relational.Model;
using Bifrons.Lenses.Symmetric.Strings;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class InsertLens : SymmetricColumnLens
{
    private readonly string _columnName;

    private InsertLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft => throw new NotImplementedException();

    public override Func<Column, Option<Column>, Result<Column>> PutRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateLeft => throw new NotImplementedException();

    public static InsertLens Cons(string columnName)
        => new(columnName);
    
    

}
