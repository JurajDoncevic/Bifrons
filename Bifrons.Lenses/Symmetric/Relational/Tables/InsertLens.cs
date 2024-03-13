using Bifrons.Lenses.Symmetric.Relational.Columns;
using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;

public sealed class InsertLens : SymmetricTableLens
{
    private readonly string _tableName;
    private readonly List<Columns.InsertLens> _symmetricColumnLenses;

    private InsertLens(string tableName, IEnumerable<Columns.InsertLens> symmetricColumnLenses)
    {
        _tableName = tableName;
        _symmetricColumnLenses = symmetricColumnLenses.ToList();
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => throw new NotImplementedException();

    public override Func<Table, Option<Table>, Result<Table>> PutRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateLeft => throw new NotImplementedException();

    public static InsertLens Cons(string tableName, IEnumerable<Columns.InsertLens>? symmetricColumnLenses = null) 
        => new(tableName, symmetricColumnLenses ?? []);
    
    public static InsertLens Cons(string tableName, params Columns.InsertLens[] symmetricColumnLenses)
        => new(tableName, symmetricColumnLenses);
}
