using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public class DisconnectLens : SymmetricColumnLens
{
    private readonly Column _leftDefault;
    private readonly Column _rightDefault;
    private readonly string _columnName;

    public override string TargetColumnName => _columnName;

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
}
