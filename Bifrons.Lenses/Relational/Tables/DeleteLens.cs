using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public sealed class DeleteLens : SymmetricTableLens
{
    private readonly string _tableName;

    public override string MatchesTableNameLeft => _tableName;
    public override string MatchesTableNameRight => Table.DEFAULT_NAME;

    public DeleteLens(string tableName)
    {
        _tableName = tableName;
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
                );

    public override Func<Table, Option<Table>, Result<Table>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
                );

    public override Func<Table, Result<Table>> CreateRight =>
        source => Table.Cons("UNIT");

    public override Func<Table, Result<Table>> CreateLeft =>
        source => Table.Cons(_tableName, source.Columns);

    public static DeleteLens Cons(string tableName)
        => new(tableName);
}
