using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;
public sealed class JoinLens : ISymmetricLens<(Table, Table), Table>
{
    private readonly string _targetTableName;
    private readonly SymmetricTableLens _leftTableLens;
    private readonly SymmetricTableLens _rightTableLens;
    private readonly Column _leftKeyColumn;
    private readonly Column _rightKeyColumn;

    public string TargetTableName => _targetTableName;

    private JoinLens(string targetTableName, SymmetricTableLens leftTableLens, SymmetricTableLens rightTableLens, Column leftKeyColumn, Column rightKeyColumn) 
    {
        _targetTableName = targetTableName;
        _leftTableLens = leftTableLens;
        _rightTableLens = rightTableLens;
        _leftKeyColumn = leftKeyColumn;
        _rightKeyColumn = rightKeyColumn;
    }

    public Func<Table, Option<(Table, Table)>, Result<(Table, Table)>> PutLeft => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => Result.Success(target),
                () => CreateLeft(updatedSource)
            );
    public Func<(Table, Table), Option<Table>, Result<Table>> PutRight => 
        (updatedSource, originalTarget) 
            => originalTarget.Match(
                target => Result.Success(Table.Cons(_targetTableName, updatedSource.Item1.Columns.Concat(updatedSource.Item2.Columns))),
                () => CreateRight(updatedSource)
            );
                

    public Func<(Table, Table), Result<Table>> CreateRight => 
        source => Table.Cons(_targetTableName, source.Item1.Columns.Concat(source.Item2.Columns));

    public Func<Table, Result<(Table, Table)>> CreateLeft => 
        source => Result.Success(
            (Table.Cons("table1", source.Columns.TakeWhile(col => !col.Equals(_leftKeyColumn))), 
             Table.Cons("table2", source.Columns.SkipWhile(col => !col.Equals(_leftKeyColumn))))
            );
}
