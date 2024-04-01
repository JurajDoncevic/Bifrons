using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Lenses.Relational.Tables;

/// <summary>
/// Table insert lens. This lens is used to insert a table.
/// insert: Table[Unit] <=> Table
/// </summary>
public sealed class InsertLens : DisconnectLens
{
    private readonly string _tableName;
    
    /// <summary>
    /// Name of the table matched by the lens
    /// </summary>
    public string TableName => _tableName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens</param>
    /// <param name="rightTableDefault">Default table for the right side</param>
    private InsertLens(string tableName, Table rightTableDefault) 
        : base(Table.UNIT_NAME, tableName, Table.ConsUnit(), rightTableDefault)
    {
        _tableName = tableName;
    }

    /// <summary>
    /// Constructs a new insert lens
    /// </summary>
    /// <param name="tableName">Name of the table matched by the lens</param>
    /// <param name="rightTableDefault">Default table for the right side</param>
    public static InsertLens Cons(string tableName, Table? rightTableDefault)
        => new(tableName, rightTableDefault ?? Table.ConsUnit());
}
