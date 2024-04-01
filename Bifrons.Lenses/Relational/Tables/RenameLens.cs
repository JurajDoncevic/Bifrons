using Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public sealed class RenameLens : IdentityLens
{
    private readonly string _leftTableName;
    private readonly string _rightTableName;
    private readonly IEnumerable<SymmetricColumnLens> _columnLenses;

    public override string MatchesTableNameLeft => _leftTableName;
    public override string MatchesTableNameRight => _rightTableName;
    public override bool MatchesLeft => true;
    public override bool MatchesRight => true;

    private RenameLens(string leftTableName, string rightTableName, IEnumerable<SymmetricColumnLens> columnLenses) 
        : base(leftTableName, columnLenses)
    {
        _leftTableName = leftTableName;
        _rightTableName = rightTableName;
        _columnLenses = columnLenses;
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
            return results.Unfold().Map(columns => Table.Cons(_leftTableName, columns.Where(col => col != UnitColumn.Cons())));
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
            return results.Unfold().Map(columns => Table.Cons(_rightTableName, columns.Where(col => col != UnitColumn.Cons())));
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
            return results.Unfold().Map(columns => Table.Cons(_rightTableName, columns.Where(col => col != UnitColumn.Cons())));
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
            return results.Unfold().Map(columns => Table.Cons(_leftTableName, columns.Where(col => col != UnitColumn.Cons())));
        };

    public static RenameLens Cons(string leftTableName, string rightTableName, IEnumerable<SymmetricColumnLens>? columnLenses = null)
        => new(leftTableName, rightTableName, columnLenses ?? []);

    public static RenameLens Cons(string leftTableName, string rightTableName, params SymmetricColumnLens[] columnLenses)
        => new(leftTableName, rightTableName, columnLenses);
}

