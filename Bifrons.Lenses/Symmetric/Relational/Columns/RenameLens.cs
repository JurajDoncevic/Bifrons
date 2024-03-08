using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class RenameLens<TSrc, TTgt> : SymmetricColumnLens
{
    private readonly string _sourceColumnName;
    private readonly string _targetColumnName;

    private RenameLens(string sourceColumnName, string targetColumnName)
    {
        _sourceColumnName = sourceColumnName;
        _targetColumnName = targetColumnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft => throw new NotImplementedException();

    public override Func<Column, Option<Column>, Result<Column>> PutRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateLeft => throw new NotImplementedException();

    public static RenameLens<TSrc, TTgt> Cons(string sourceColumnName, string targetColumnName)
        => new(sourceColumnName, targetColumnName);
    

}