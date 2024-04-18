using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;
public sealed class InnerJoinLens : ISymmetricLens<(Table, Table), Table>
{
    private readonly Column _leftKey;
    private readonly Column _rightKey;
    private readonly string _joinedTableName;
    private readonly string _defaultLeftTableName;
    private readonly string _defaultRightTableName;

    private InnerJoinLens(string joinedTableName, Column leftKey, Column rightKey, string defaultLeftTableName = UnitColumn.UNIT_NAME, string defaultRightTableName = UnitColumn.UNIT_NAME)
    {
        _leftKey = leftKey;
        _rightKey = rightKey;
        _joinedTableName = joinedTableName;
        _defaultLeftTableName = defaultLeftTableName;
        _defaultRightTableName = defaultRightTableName;
    }

    public Func<Table, Option<(Table, Table)>, Result<(Table, Table)>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => updatedSource.Columns.Contains(_leftKey) && updatedSource.Columns.Contains(_rightKey)
                ? updatedSource.Columns.Fold(
                    (Enumerable.Empty<Column>(), Enumerable.Empty<Column>()), 
                    (col, sepCols) => target.Item1.Columns.Contains(col) 
                        ? (sepCols.Item1.Append(col), sepCols.Item2) 
                        : target.Item2.Columns.Contains(col) 
                            ? (sepCols.Item1, sepCols.Item2.Append(col)) 
                            : sepCols
                    ).ToIdentity()
                    .Map(sepCols => (Table.Cons(target.Item1.Name, sepCols.Item1), Table.Cons(target.Item2.Name, sepCols.Item2)))
                    .Data
                : Result.Failure<(Table, Table)>($"Table {updatedSource.Name} does not contain the specified keys: {_leftKey} and {_rightKey}."),
            () => CreateLeft(updatedSource)
        );

    public Func<(Table, Table), Option<Table>, Result<Table>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => updatedSource.Item1.Columns.Contains(_leftKey) && updatedSource.Item2.Columns.Contains(_rightKey)
                ? Table.Cons(target.Name, updatedSource.Item1.Columns.Concat(updatedSource.Item2.Columns))
                : Result.Failure<Table>($"Tables {updatedSource.Item1.Name} and {updatedSource.Item2.Name} do not contain the specified keys: {_leftKey} and {_rightKey}."),
            () => CreateRight(updatedSource)
        );

    public Func<(Table, Table), Result<Table>> CreateRight => 
        source => source.Item1.Columns.Contains(_leftKey) && source.Item2.Columns.Contains(_rightKey)
            ? Table.Cons(_joinedTableName, source.Item1.Columns.Concat(source.Item2.Columns))
            : Result.Failure<Table>($"Tables {source.Item1.Name} and {source.Item2.Name} do not contain the specified keys: {_leftKey} and {_rightKey}."); 

    public Func<Table, Result<(Table, Table)>> CreateLeft => 
        source => Result.Success(
                (
                    Table.Cons(_defaultLeftTableName, source.Columns.TakeWhile(col => col != _rightKey)),
                    Table.Cons(_defaultRightTableName, source.Columns.SkipWhile(col => col != _rightKey))
                )
        );

    public static InnerJoinLens Cons(string joinedTableName, Column leftKey, Column rightKey)
        => new(joinedTableName, leftKey, rightKey, "People", "Departments");
}
