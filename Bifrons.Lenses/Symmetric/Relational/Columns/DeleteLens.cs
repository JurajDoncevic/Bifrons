using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class DeleteLens : SymmetricColumnLens
{
    private readonly string _columnName;

    private DeleteLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft => throw new NotImplementedException();

    public override Func<Column, Option<Column>, Result<Column>> PutRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateRight => throw new NotImplementedException();

    public override Func<Column, Result<Column>> CreateLeft => throw new NotImplementedException();

    public static DeleteLens Cons(string columnName)
        => new(columnName);

}

