using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column rename lens. This lens is used to represent a column that is renamed during a transformation.
/// rename: Column <=> Column
/// </summary>
public sealed class RenameLens : IdentityLens
{
    private readonly string _leftColumnName;
    private readonly string _rightColumnName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="leftColumnName">Left column name</param>
    /// <param name="rightColumnName">Right column name</param>
    private RenameLens(string leftColumnName, string rightColumnName) : base(leftColumnName)
    {
        _leftColumnName = leftColumnName ?? Guid.NewGuid().ToString();
        _rightColumnName = rightColumnName ?? Guid.NewGuid().ToString();
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(Column.Cons(_leftColumnName, target.DataType)),
                () => CreateLeft(updatedSource)
            );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(Column.Cons(_rightColumnName, target.DataType)),
                () => CreateRight(updatedSource)
            );

    public override Func<Column, Result<Column>> CreateRight =>
        source => Result.Success(Column.Cons(_rightColumnName, source.DataType));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(Column.Cons(_leftColumnName, source.DataType));

    /// <summary>
    /// Constructs a new RenameLens
    /// </summary>
    /// <param name="leftColumnName">Left column name</param>
    /// <param name="rightColumnName">Right column name</param>
    public static RenameLens Cons(string leftColumnName, string rightColumnName)
        => new(leftColumnName, rightColumnName);


}