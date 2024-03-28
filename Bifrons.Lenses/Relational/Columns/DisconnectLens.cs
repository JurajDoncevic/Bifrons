using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Columns;

/// <summary>
/// Column disconnect lens. This lens is used to represent a column that is disconnected during a transformation.
/// disconnect: Column <=> Column
/// </summary>
public class DisconnectLens : SymmetricColumnLens
{
    private readonly Column _leftDefault;
    private readonly Column _rightDefault;
    private readonly string _columnName;

    public override string TargetColumnName => _columnName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="columnName">Target column name</param>
    /// <param name="leftDefault">Default column for the left side</param>
    /// <param name="rightDefault">Default column for the right side</param>
    protected DisconnectLens(string columnName, Column leftDefault, Column rightDefault)
    {
        _columnName = columnName;
        _rightDefault = rightDefault;
        _leftDefault = leftDefault;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
            );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
            );
    public override Func<Column, Result<Column>> CreateRight =>
        source => Result.Success(_rightDefault);

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(_leftDefault);

    /// <summary>
    /// Constructs a new DisconnectLens
    /// </summary>
    /// <param name="columnName">Target column name</param>
    /// <param name="leftDefault">Default column for the left side</param>
    /// <param name="rightDefault">Default column for the right side</param>
    public static DisconnectLens Cons(string columnName, Column leftDefault, Column rightDefault)
        => new(columnName, leftDefault, rightDefault);
}
