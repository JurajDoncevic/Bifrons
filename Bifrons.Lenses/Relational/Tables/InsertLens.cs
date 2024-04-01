using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public sealed class InsertLens : DisconnectLens
{
    private readonly string _tableName;
    public string TableName => _tableName;

    public InsertLens(string tableName, Table rightTableDefault) 
        : base(Table.UNIT_NAME, tableName, Table.ConsUnit(), rightTableDefault)
    {
        _tableName = tableName;
    }

    public static InsertLens Cons(string tableName, Table? rightTableDefault)
        => new(tableName, rightTableDefault ?? Table.ConsUnit());
}
