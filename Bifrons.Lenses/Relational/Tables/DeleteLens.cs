using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

public sealed class DeleteLens : DisconnectLens
{
    private readonly string _tableName;
    public string TableName => _tableName;

    private DeleteLens(string tableName)
        : base(tableName, Table.UNIT_NAME, Table.ConsUnit(tableName), Table.ConsUnit())
    {
        _tableName = tableName;
    }

    public static DeleteLens Cons(string tableName)
        => new(tableName);
}
