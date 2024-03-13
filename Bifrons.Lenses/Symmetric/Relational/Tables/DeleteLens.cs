using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public sealed class DeleteLens : SymmetricTableLens
{
    private readonly string _tableName;

    public DeleteLens(string tableName)
    {
        _tableName = tableName;
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => throw new NotImplementedException();

    public override Func<Table, Option<Table>, Result<Table>> PutRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateLeft => throw new NotImplementedException();

    public static DeleteLens Cons(string tableName) 
        => new(tableName);
}
