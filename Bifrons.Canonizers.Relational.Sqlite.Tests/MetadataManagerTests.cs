using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Canonizers.Relational.Sqlite.Tests;

public class MetadataManagerTests
{
    private readonly MetadataManager _metadataManager;

    public MetadataManagerTests()
    {
        _metadataManager = MetadataManager.Cons("Data Source=./PeopleAndRoles.db;").Data ?? throw new Exception("Failed to create metadata manager");
    }

    [Fact(DisplayName = "Get metadata for a table")]
    public void GetTableMetadata_WhenTableExists_ReturnsTable()
    {
        var tableResult = _metadataManager.GetTable("Person");
        var table = tableResult.Data;
        Assert.True(tableResult);
        Assert.Equal("Person", table.Name);
        Assert.Equal(4, table.Columns.Count);
        Assert.Equal("Id", table.Columns[0].Name);
        Assert.Equal(DataTypes.LONG, table.Columns[0].DataType);
        Assert.Equal("FirstName", table.Columns[1].Name);
        Assert.Equal(DataTypes.STRING, table.Columns[1].DataType);
        Assert.Equal("LastName", table.Columns[2].Name);
        Assert.Equal(DataTypes.STRING, table.Columns[2].DataType);
        Assert.Equal("DateOfBirth", table.Columns[3].Name);
        Assert.Equal(DataTypes.DATETIME, table.Columns[3].DataType);
    }

    [Fact(DisplayName = "Get metadata for all tables")]
    public void GetAllTableMetadata_WhenTablesExist_ReturnsTables()
    {
        var tablesResult = _metadataManager.GetAllTables();
        var tables = tablesResult.Data.ToList();

        Assert.True(tablesResult);
        Assert.Equal(3, tables.Count());
        Assert.Equal("Person", tables[0].Name);
        Assert.Equal(4, tables[0].Columns.Count);
        Assert.Equal("Role", tables[1].Name);
        Assert.Equal(2, tables[1].Columns.Count);
        Assert.Equal("PersonRole", tables[2].Name);
        Assert.Equal(4, tables[2].Columns.Count);
    }

    [Fact(DisplayName = "Create a table and drop it")]
    public void CreateAndDropTable_WhenTableNotExists()
    {
        var table = Table.Cons(
            "TestTable",
            Column.Cons("Id", DataTypes.INTEGER),
            Column.Cons("StringATTR", DataTypes.STRING),
            Column.Cons("IntATTR", DataTypes.INTEGER),
            Column.Cons("DateTimeATTR", DataTypes.DATETIME),
            Column.Cons("BoolATTR", DataTypes.BOOLEAN),
            Column.Cons("DecimalATTR", DataTypes.DECIMAL)
        );

        var createResult = _metadataManager.CreateTable(table);
        var dropResult = _metadataManager.DropTable(table.Name);

        Assert.True(createResult);
        Assert.True(dropResult);
    }
}
