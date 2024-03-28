using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Relational.Columns;
namespace Bifrons.Lenses.Relational.Tables;

public class IdentityLens : SymmetricTableLens
{
    private readonly string _tableName;
    private readonly List<SymmetricColumnLens> _symmetricColumnLenses;

    public override string TargetTableName => _tableName;
    public IReadOnlyList<SymmetricColumnLens> SymmetricColumnLenses => _symmetricColumnLenses;

    protected IdentityLens(string tableName, IEnumerable<SymmetricColumnLens> symmetricColumnLenses)
    {
        _tableName = tableName;
        _symmetricColumnLenses = symmetricColumnLenses.ToList();
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => 
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                original =>  
                    updatedSource.Columns.Count == _symmetricColumnLenses.Count && 
                        ? 
                        : Result.Failure<Table>($"The number of columns in the source table '{updatedSource.Name}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'"),
                () => CreateLeft(updatedSource)
            );

    public override Func<Table, Option<Table>, Result<Table>> PutRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateRight =>
        source => source.Columns.Count == _symmetricColumnLenses.Where(lens => lens is not Columns.InsertLens).Count() // identity, rename, and delete lenses refer to the source columns
            ? source.Columns.Mapi((i, column) => _symmetricColumnLenses[i].CreateRight(column))
                .Unfold()
                .Map(columns => Table.Cons(_tableName, columns))
            : Result.Failure<Table>($"The number of columns in the source table '{source.Name}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'");

    public override Func<Table, Result<Table>> CreateLeft => 
        source => source.Columns.Count == _symmetricColumnLenses.Where(lens => lens is not Columns.DeleteLens).Count() // identity, rename, and insert lenses refer to the source columns
            ? source.Columns.Mapi((i, column) => _symmetricColumnLenses[i].CreateLeft(column))
                .Unfold()
                .Map(columns => Table.Cons(_tableName, columns))
            : Result.Failure<Table>($"The number of columns in the source table '{source.Name}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'");

    public static IdentityLens Cons(string tableName, IEnumerable<SymmetricColumnLens>? symmetricColumnLenses = null)
        => new(tableName, symmetricColumnLenses ?? []);

    public static IdentityLens Cons(string tableName, params SymmetricColumnLens[] symmetricColumnLenses)
        => new(tableName, symmetricColumnLenses);
}
