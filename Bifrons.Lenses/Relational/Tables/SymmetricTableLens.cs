using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public abstract class SymmetricTableLens : ISymmetricLens<Table, Table>
{
    public abstract string MatchesTableNameLeft { get; }
    public abstract string MatchesTableNameRight { get; }

    public abstract bool MatchesLeft { get; }
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
