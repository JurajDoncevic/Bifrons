using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

/// <summary>
/// Represents a simple symmetric lens between two tables.
/// </summary>
public abstract class SymmetricTableLens : ISymmetricLens<Table, Table>
{
    /// <summary>
    /// Name of the table on the left side that the lens matches.
    /// </summary>
    public abstract string MatchesTableNameLeft { get; }
    /// <summary>
    /// Name of the table on the right side that the lens matches.
    /// </summary>
    public abstract string MatchesTableNameRight { get; }
    /// <summary>
    /// Whether the lens matches a specific table on the left side.
    /// </summary>
    public abstract bool MatchesLeft { get; }
    /// <summary>
    /// Whether the lens matches a specific table the right side.
    /// </summary>
    public abstract bool MatchesRight { get; }

    public abstract Func<Table, Option<Table>, Result<Table>> PutLeft { get; }
    public abstract Func<Table, Option<Table>, Result<Table>> PutRight { get; }
    public abstract Func<Table, Result<Table>> CreateRight { get; }
    public abstract Func<Table, Result<Table>> CreateLeft { get; }

    public override string ToString() 
    {
        return $"[{this.GetType().Name[..^4]}(" 
                + (MatchesLeft ? $"'{MatchesTableNameLeft}'" : "")
                + (MatchesLeft && MatchesRight ? ", " : "")
                + (MatchesRight ? $"'{MatchesTableNameRight}'" : "")
                + "): Table <=> Table]";
    }
}
