using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class InsertLens : SymmetricColumnLens
{
    private readonly string _columnName;

    public override string TargetColumnName => _columnName;

    private InsertLens(string columnName)
    {
        _columnName = columnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
                );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                );

    public override Func<Column, Result<Column>> CreateRight =>
        source => Result.Success(Column.Cons(_columnName, source.DataType));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(UnitColumn.Cons(_columnName) as Column);

    public static InsertLens Cons(string columnName)
        => new(columnName);



}
