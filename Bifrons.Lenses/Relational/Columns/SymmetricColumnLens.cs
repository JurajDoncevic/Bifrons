using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Abstract class for a symmetric lens that operates on columns.
/// L : Column <=> Column
/// </summary>
public abstract class SymmetricColumnLens : ISymmetricLens<Column, Column>
{
    /// <summary>
    /// The name of the column that this lens operates on the Left.
    /// </summary>
    public abstract string MatchesColumnNameLeft { get; }
    /// <summary>
    /// The name of the column that this lens operates on the Right.
    /// </summary>
    public abstract string MatchesColumnNameRight { get; }

    /// <summary>
    /// Whether the lens matches a specific column on the Left.
    /// </summary>
    public abstract bool MatchesLeft { get; }

    /// <summary>
    /// Whether the lens matches a specific column on the Right.
    /// </summary>
    public abstract bool MatchesRight { get; }

    public abstract Func<Column, Option<Column>, Result<Column>> PutLeft { get; }
    public abstract Func<Column, Option<Column>, Result<Column>> PutRight { get; }
    public abstract Func<Column, Result<Column>> CreateRight { get; }
    public abstract Func<Column, Result<Column>> CreateLeft { get; }
}

