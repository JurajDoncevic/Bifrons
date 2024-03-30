using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Relational.Columns;
namespace Bifrons.Lenses.Relational.Tables;

public class IdentityLens : SymmetricTableLens
{
    private readonly string _tableName;
    private readonly List<SymmetricColumnLens> _columnLenses;

    public override string MatchesTableNameLeft => _tableName;
    public override string MatchesTableNameRight => _tableName;

    public IReadOnlyList<SymmetricColumnLens> ColumnLenses => _columnLenses;

    protected IdentityLens(string tableName, IEnumerable<SymmetricColumnLens> columnLenses)
    {
        _tableName = tableName;
        _columnLenses = columnLenses.ToList();
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => 
        (source, target) => target.Match(
            t => t.Columns.Count == _columnLenses.Count(l => l.MatchesRight) // identity, rename, and insert lenses refer to the target columns
                ? _columnLenses.Fold(
                        Enumerable.Empty<Result<Column>>(),
                        (lens, columns) => columns.Append(
                            t[lens.MatchesColumnNameRight].Match(
                                col => lens.PutLeft(col, source[lens.MatchesColumnNameLeft]), 
                                () => Result.Failure<Column>($"Column '{lens.MatchesColumnNameRight}' not found for lens"))
                            )
                        )
                    .Unfold()
                    .Map(columns => Table.Cons(_tableName, columns))
                : Result.Failure<Table>($"The number of columns in the target table '{_tableName}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'"),
            () => CreateLeft(source)
            );

    public override Func<Table, Option<Table>, Result<Table>> PutRight =>
        (source, target) => target.Match(
            t => t.Columns.Count == _columnLenses.Count(l => l.MatchesLeft) // identity, rename, and delete lenses refer to the source columns
                ? _columnLenses.Fold(
                        Enumerable.Empty<Result<Column>>(),
                        (lens, columns) => columns.Append(
                            t[lens.MatchesColumnNameLeft].Match(
                                col => lens.PutRight(col, source[lens.MatchesColumnNameRight]), 
                                () => Result.Failure<Column>($"Column '{lens.MatchesColumnNameLeft}' not found"))
                            )
                        )
                    .Unfold()
                    .Map(columns => Table.Cons(_tableName, columns))
                : Result.Failure<Table>($"The number of columns in the target table '{_tableName}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'"),
            () => CreateRight(source)
            );

    public override Func<Table, Result<Table>> CreateRight =>
        source => source.Columns.Count == _columnLenses.Count(l => l.MatchesLeft) // identity, rename, and delete lenses refer to the source columns
            ? _columnLenses.Fold(
                    Enumerable.Empty<Result<Column>>(),
                    (lens, columns) => columns.Append(
                        source[lens.MatchesColumnNameLeft].Match(
                            col => lens.CreateRight(col), 
                            () => Result.Failure<Column>($"Column '{lens.MatchesColumnNameLeft}' not found"))
                        )
                    )
                .Unfold()
                .Map(columns => Table.Cons(_tableName, columns))
            : Result.Failure<Table>($"The number of columns in the source table '{source.Name}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'");

    public override Func<Table, Result<Table>> CreateLeft => 
        source => source.Columns.Count == _columnLenses.Count(l => l.MatchesRight) // identity, rename, and insert lenses refer to the target columns
            ? _columnLenses.Fold(
                    Enumerable.Empty<Result<Column>>(),
                    (lens, columns) => columns.Append(
                        source[lens.MatchesColumnNameRight].Match(
                            col => lens.CreateLeft(col), 
                            () => Result.Failure<Column>($"Column '{lens.MatchesColumnNameRight}' not found for lens"))
                        )
                    )
                .Unfold()
                .Map(columns => Table.Cons(_tableName, columns))
            : Result.Failure<Table>($"The number of columns in the source table '{source.Name}' does not match the number of symmetric column lenses in the identity lens targeting '{_tableName}'");

    public static IdentityLens Cons(string tableName, IEnumerable<SymmetricColumnLens>? columnLenses = null)
        => new(tableName, columnLenses ?? []);

    public static IdentityLens Cons(string tableName, params SymmetricColumnLens[] columnLenses)
        => new(tableName, columnLenses);
}
