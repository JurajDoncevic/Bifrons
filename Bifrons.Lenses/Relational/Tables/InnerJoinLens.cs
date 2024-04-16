using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;
public sealed class InnerJoinLens : ISymmetricLens<(Table, Table), Table>
{
    private readonly Column _leftKey;
    private readonly Column _rightKey;
    private readonly string _joinedTableName;

    private InnerJoinLens(string joinedTableName, Column leftKey, Column rightKey)
    {
        _leftKey = leftKey;
        _rightKey = rightKey;
        _joinedTableName = joinedTableName;
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
                    .Map(sepCols => (Table.Cons(_joinedTableName, sepCols.Item1.Append(_leftKey)), Table.Cons(_joinedTableName, sepCols.Item2.Append(_rightKey))))
                    .Data
                : Result.Failure<(Table, Table)>($"Table {updatedSource.Name} does not contain the specified keys: {_leftKey} and {_rightKey}."),
            () => CreateLeft(updatedSource)
        );

    public Func<(Table, Table), Option<Table>, Result<Table>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            target => updatedSource.Item1.Columns.Contains(_leftKey) && updatedSource.Item2.Columns.Contains(_rightKey)
                ? Table.Cons(_joinedTableName, updatedSource.Item1.Columns.Concat(updatedSource.Item2.Columns))
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
                    Table.Cons(_joinedTableName, source.Columns.Where(col => col != _rightKey || col != _leftKey).Append(_leftKey)),
                    Table.Cons(_joinedTableName, source.Columns.Where(col => col != _leftKey || col != _rightKey).Append(_rightKey))
                )
        );

    public static InnerJoinLens Cons(string joinedTableName, Column leftKey, Column rightKey)
        => new(joinedTableName, leftKey, rightKey);
}
