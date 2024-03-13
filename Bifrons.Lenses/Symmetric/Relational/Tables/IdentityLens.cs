using Bifrons.Lenses.Symmetric.Relational.Model;
using Bifrons.Lenses.Symmetric.Relational.Columns;
namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public sealed class IdentityLens : SymmetricTableLens
{
    private readonly string _tableName;
    private readonly List<SymmetricColumnLens> _symmetricColumnLenses;

    private IdentityLens(string tableName, IEnumerable<SymmetricColumnLens> symmetricColumnLenses)
    {
        _tableName = tableName;
        _symmetricColumnLenses = symmetricColumnLenses.ToList();
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => throw new NotImplementedException();

    public override Func<Table, Option<Table>, Result<Table>> PutRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateLeft => throw new NotImplementedException();

    public static IdentityLens Cons(string tableName, IEnumerable<SymmetricColumnLens>? symmetricColumnLenses = null) 
        => new(tableName, symmetricColumnLenses ?? []);

    public static IdentityLens Cons(string tableName, params SymmetricColumnLens[] symmetricColumnLenses) 
        => new(tableName, symmetricColumnLenses);
}
