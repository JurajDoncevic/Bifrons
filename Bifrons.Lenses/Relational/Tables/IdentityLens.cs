using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.Relational.Columns;
namespace Bifrons.Lenses.Relational.Tables;

/// <summary>
/// Table identity lens. This lens is used to "copy" a table.
/// id: Table <=> Table
/// </summary>
public class IdentityLens : SymmetricTableLens
{
    private readonly string _tableName;
    private readonly List<SymmetricColumnLens> _columnLenses;

    public override string MatchesTableNameLeft => _tableName;
    public override string MatchesTableNameRight => _tableName;
    public override bool MatchesLeft => true;
    public override bool MatchesRight => true;

    /// <summary>
    /// Name of the table matched by the lens.
    /// </summary>
    public string TableName => _tableName;
    /// <summary>
    /// Column lenses that match and transform columns between the tables.
    /// </summary>
    public IReadOnlyList<SymmetricColumnLens> ColumnLenses => _columnLenses;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens.</param>
    /// <param name="columnLenses">Column lenses that match and transform columns between the tables.</param>
    protected IdentityLens(string tableName, IEnumerable<SymmetricColumnLens> columnLenses)
    {
        _tableName = tableName;
        _columnLenses = columnLenses.ToList();
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateLeft(updatedSource);
            }
            var results = Enumerable.Empty<Result<Column>>();
            foreach (var lens in _columnLenses)
            {
                if (lens.MatchesRight)
                {
                    if (!updatedSource[lens.MatchesColumnNameRight])
                    {
                        return Result.Failure<Table>($"Column '{lens.MatchesColumnNameRight}' not found for lens {this}.");
                    }

                    results = lens.PutLeft(updatedSource[lens.MatchesColumnNameRight].Value, originalTarget.Value[lens.MatchesColumnNameLeft]).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while putting left column '{lens.MatchesColumnNameRight}'.")));
                }
                else
                {
                    results = lens.PutLeft(UnitColumn.Cons(), originalTarget.Value[lens.MatchesColumnNameLeft]).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while putting left column '{lens.MatchesColumnNameLeft}'.")));
                }
            }
            return results.Unfold().Map(columns => Table.Cons(_tableName, columns.Where(col => col != UnitColumn.Cons())));
        };

    public override Func<Table, Option<Table>, Result<Table>> PutRight =>
        (updatedSource, originalTarget) =>
        {
            if (!originalTarget)
            {
                return CreateRight(updatedSource);
            }
            var results = Enumerable.Empty<Result<Column>>();
            foreach (var lens in _columnLenses)
            {
                if (lens.MatchesLeft)
                {
                    if (!updatedSource[lens.MatchesColumnNameLeft])
                    {
                        return Result.Failure<Table>($"Column '{lens.MatchesColumnNameLeft}' not found for lens {this}.");
                    }

                    results = lens.PutRight(updatedSource[lens.MatchesColumnNameLeft].Value, originalTarget.Value[lens.MatchesColumnNameRight]).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while putting right column '{lens.MatchesColumnNameLeft}'."))
                    );
                }
                else
                {
                    results = lens.PutRight(UnitColumn.Cons(), originalTarget.Value[lens.MatchesColumnNameRight]).Match(
                        col => results = results.Append(Result.Success(col)),
                        msg => results = results.Append(Result.Failure<Column>($"Error while putting right column '{lens.MatchesColumnNameRight}'."))
                    );
                }
            }
            return results.Unfold().Map(columns => Table.Cons(_tableName, columns.Where(col => col != UnitColumn.Cons())));
        };


    public override Func<Table, Result<Table>> CreateRight =>
        source =>
        {
            var results = Enumerable.Empty<Result<Column>>();
            foreach (var lens in _columnLenses)
            {
                if (lens.MatchesLeft)
                {
                    if (!source[lens.MatchesColumnNameLeft])
                    {
                        return Result.Failure<Table>($"Column '{lens.MatchesColumnNameLeft}' not found for lens {this}.");
                    }

                    results = lens.CreateRight(source[lens.MatchesColumnNameLeft].Value).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while creating right column '{lens.MatchesColumnNameLeft}'."))
                    );
                }
                else
                {
                    results = lens.CreateRight(UnitColumn.Cons()).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while creating right column '{lens.MatchesColumnNameRight}'."))
                    );
                }
            }
            return results.Unfold().Map(columns => Table.Cons(_tableName, columns.Where(col => col != UnitColumn.Cons())));
        };

    public override Func<Table, Result<Table>> CreateLeft =>
        source =>
        {
            var results = Enumerable.Empty<Result<Column>>();
            foreach (var lens in _columnLenses)
            {
                if (lens.MatchesRight)
                {
                    if (!source[lens.MatchesColumnNameRight])
                    {
                        return Result.Failure<Table>($"Column '{lens.MatchesColumnNameRight}' not found for lens {this}.");
                    }

                    results = lens.CreateLeft(source[lens.MatchesColumnNameRight].Value).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while creating left column '{lens.MatchesColumnNameRight}'."))
                    );
                }
                else
                {
                    results = lens.CreateLeft(UnitColumn.Cons()).Match(
                        col => results.Append(Result.Success(col)),
                        msg => results.Append(Result.Failure<Column>($"Error while creating left column '{lens.MatchesColumnNameLeft}'."))
                    );
                }
            }
            return results.Unfold().Map(columns => Table.Cons(_tableName, columns.Where(col => col != UnitColumn.Cons())));
        };

    /// <summary>
    /// Constructs a new identity lens
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens</param>
    /// <param name="columnLenses">Column lenses that match and transform columns between the tables</param>
    public static IdentityLens Cons(string tableName, IEnumerable<SymmetricColumnLens>? columnLenses = null)
        => new(tableName, columnLenses ?? []);

    /// <summary>
    /// Constructs a new identity lens
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens</param>
    /// <param name="columnLenses">Column lenses that match and transform columns between the tables</param>
    public static IdentityLens Cons(string tableName, params SymmetricColumnLens[] columnLenses)
        => new(tableName, columnLenses);
}
