using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class DeleteLens : SymmetricColumnLens
{
    private readonly string _columnName;

    public override string TargetColumnName => _columnName;

    private DeleteLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                source => Result.Success(source),
                () => Result.Success(UnitColumn.Cons(_columnName) as Column)
                );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                source => Result.Success(source),
                () => Result.Success(UnitColumn.Cons(_columnName) as Column)
                );

    public override Func<Column, Result<Column>> CreateRight =>
        source => UnitColumn.Cons(_columnName);

    public override Func<Column, Result<Column>> CreateLeft =>
        source => source;

    public static DeleteLens Cons(string columnName)
        => new(columnName);

}

