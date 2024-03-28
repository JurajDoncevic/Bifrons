using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

public abstract class SymmetricColumnLens : ISymmetricLens<Column, Column>
{
    public abstract string TargetColumnName { get; }
    public abstract Func<Column, Option<Column>, Result<Column>> PutLeft { get; }
    public abstract Func<Column, Option<Column>, Result<Column>> PutRight { get; }
    public abstract Func<Column, Result<Column>> CreateRight { get; }
    public abstract Func<Column, Result<Column>> CreateLeft { get; }
}

