using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public sealed class InsertLens : SymmetricTableLens
{
    private readonly string _tableName;
    private readonly List<Columns.InsertLens> _symmetricColumnLenses;

    public override string TargetTableName => _tableName;
    public IReadOnlyList<Columns.InsertLens> SymmetricColumnLenses => _symmetricColumnLenses;

    private InsertLens(string tableName, IEnumerable<Columns.InsertLens> symmetricColumnLenses)
    {
        _tableName = tableName;
        _symmetricColumnLenses = symmetricColumnLenses.ToList();
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
        source => Table.Cons(_tableName, source.Columns);

    public override Func<Table, Result<Table>> CreateLeft =>
        source => Table.Cons("UNIT");

    public static InsertLens Cons(string tableName, IEnumerable<Columns.InsertLens>? symmetricColumnLenses = null)
        => new(tableName, symmetricColumnLenses ?? []);

    public static InsertLens Cons(string tableName, params Columns.InsertLens[]? symmetricColumnLenses)
        => new(tableName, symmetricColumnLenses ?? []);
}
