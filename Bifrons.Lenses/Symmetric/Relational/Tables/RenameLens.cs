using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public sealed class RenameLens : SymmetricTableLens
{
    private readonly string _sourceTableName;
    private readonly string _destinationTableName;

    private RenameLens(string sourceTableName, string destinationTableName)
    {
        _sourceTableName = sourceTableName;
        _destinationTableName = destinationTableName;
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => throw new NotImplementedException();

    public override Func<Table, Option<Table>, Result<Table>> PutRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateLeft => throw new NotImplementedException();

    public static RenameLens Cons(string sourceTableName, string destinationTableName) 
        => new(sourceTableName, destinationTableName);
}
