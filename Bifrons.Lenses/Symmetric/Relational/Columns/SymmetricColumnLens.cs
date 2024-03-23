using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public abstract class SymmetricColumnLens : ISimpleSymmetricLens<Column, Column>
{
    public abstract string TargetColumnName { get; }
    public abstract Func<Column, Option<Column>, Result<Column>> PutLeft { get; }
    public abstract Func<Column, Option<Column>, Result<Column>> PutRight { get; }
    public abstract Func<Column, Result<Column>> CreateRight { get; }
    public abstract Func<Column, Result<Column>> CreateLeft { get; }
}

