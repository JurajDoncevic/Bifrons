using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Tables;
public sealed class JoinLens : SymmetricTableLens
{
    private readonly string _targetTableName;
    private readonly SymmetricTableLens _leftTableLens;
    private readonly SymmetricTableLens _rightTableLens;
    private readonly Column _leftKeyColumn;
    private readonly Column _rightKeyColumn;

    public override string TargetTableName => _targetTableName;

    private JoinLens(string targetTableName, SymmetricTableLens leftTableLens, SymmetricTableLens rightTableLens, Column leftKeyColumn, Column rightKeyColumn) 
    {
        _targetTableName = targetTableName;
        _leftTableLens = leftTableLens;
        _rightTableLens = rightTableLens;
        _leftKeyColumn = leftKeyColumn;
        _rightKeyColumn = rightKeyColumn;
    }


    public override Func<Table, Option<Table>, Result<Table>> PutLeft => throw new NotImplementedException();

    public override Func<Table, Option<Table>, Result<Table>> PutRight => throw new NotImplementedException();

    public override Func<Table, Result<Table>> CreateRight => 
        table => _rightTableLens.CreateRight(table).Map(rightTable => Table.Cons(TargetTableName, rightTable.Columns));

    public override Func<Table, Result<Table>> CreateLeft => throw new NotImplementedException();
}
