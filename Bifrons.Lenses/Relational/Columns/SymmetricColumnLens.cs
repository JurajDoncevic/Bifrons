using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Abstract class for a symmetric lens that operates on columns.
/// L : Column <=> Column
/// </summary>
public abstract class SymmetricColumnLens : ISymmetricLens<Column, Column>
{
    /// <summary>
    /// The name of the column that this lens operates on.
    /// </summary>
    public abstract string TargetColumnName { get; }

    public abstract Func<Column, Option<Column>, Result<Column>> PutLeft { get; }
    public abstract Func<Column, Option<Column>, Result<Column>> PutRight { get; }
    public abstract Func<Column, Result<Column>> CreateRight { get; }
    public abstract Func<Column, Result<Column>> CreateLeft { get; }
}

