using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public class DisconnectLens : SymmetricTableLens
{
    private readonly string _leftTableName;
    private readonly string _rightTableName;
    private readonly Table _leftTableDefault;
    private readonly Table _rightTableDefault;

    public override string MatchesTableNameLeft => _leftTableName;
    public override string MatchesTableNameRight => _rightTableName;
    public override bool MatchesLeft => !_leftTableDefault.IsUnit;
    public override bool MatchesRight => !_rightTableDefault.IsUnit;

    protected DisconnectLens(string leftTableName, string rightTableName, Table leftTableDefault, Table rightTableDefault)
    {
        _leftTableName = leftTableName;
        _rightTableName = rightTableName;
        _leftTableDefault = leftTableDefault;
        _rightTableDefault = rightTableDefault;
    }

    public override Func<Table, Option<Table>, Result<Table>> PutLeft => 
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
            );

    public override Func<Table, Option<Table>, Result<Table>> PutRight => 
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(target),
                () => CreateRight(updatedSource)
            );

    public override Func<Table, Result<Table>> CreateRight =>
        source => Result.Success(_rightTableDefault);

    public override Func<Table, Result<Table>> CreateLeft => 
        source => Result.Success(_leftTableDefault);


    public static DisconnectLens Cons(string leftTableName, string rightTableName, Table leftTableDefault, Table rightTableDefault)
        => new(leftTableName, rightTableName, leftTableDefault, rightTableDefault);
}
