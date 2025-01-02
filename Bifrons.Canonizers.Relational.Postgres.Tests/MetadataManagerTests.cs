using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Canonizers.Relational.Postgres.Tests;

[Collection("Database collection")]
public class MetadataManagerTests
{
    private readonly DatabaseFixture _fixture;

    public MetadataManagerTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Get metadata for a table")]
    public void GetTableMetadata_WhenTableExists_ReturnsTable()
    {
        var metadataManager = _fixture.GetService<MetadataManager>();

        var tableResult = metadataManager.GetTable("Person");
        var table = tableResult.Data;
        Assert.True(tableResult);
        Assert.Equal("Person", table.Name);
        Assert.Equal(4, table.Columns.Count);
        Assert.Contains(table.Columns, c => c.Name == "Id");
        Assert.Contains(table.Columns, c => c.Name == "FirstName");
        Assert.Contains(table.Columns, c => c.Name == "LastName");
        Assert.Contains(table.Columns, c => c.Name == "DateOfBirth");
        Assert.Equal(DataTypes.INTEGER, table["Id"].Value.DataType);
        Assert.Equal(DataTypes.STRING, table["FirstName"].Value.DataType);
        Assert.Equal(DataTypes.STRING, table["LastName"].Value.DataType);
        Assert.Equal(DataTypes.DATETIME, table["DateOfBirth"].Value.DataType);
    }

    [Fact(DisplayName = "Get metadata for all tables")]
    public void GetAllTableMetadata_WhenTablesExist_ReturnsTables()
    {
        var metadataManager = _fixture.GetService<MetadataManager>();

        var tablesResult = metadataManager.GetAllTables();
        var tables = tablesResult.Data?.ToList() ?? throw new Exception($"Failed to get tables: {tablesResult.Message}");

        Assert.True(tablesResult);
        Assert.Equal(3, tables.Count());
        Assert.Contains(tables, t => t.Name == "Person");
        Assert.Contains(tables, t => t.Name == "Role");
        Assert.Contains(tables, t => t.Name == "PersonRole");
        Assert.Equal(4, tables.Single(t => t.Name == "Person").Columns.Count);
        Assert.Equal(2, tables.Single(t => t.Name == "Role").Columns.Count);
        Assert.Equal(4, tables.Single(t => t.Name == "PersonRole").Columns.Count);
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
        var metadataManager = _fixture.GetService<MetadataManager>();

        var tableExistsPre = metadataManager.TableExists(table.Name);
        var createResult = metadataManager.CreateTable(table);
        var tableExistsPost = metadataManager.TableExists(table.Name);
        var dropResult = metadataManager.DropTable(table.Name);
        var tableExistsResult = metadataManager.TableExists(table.Name);

        Assert.False(tableExistsPre);
        Assert.True(createResult);
        Assert.True(tableExistsPost);
        Assert.True(dropResult);
        Assert.False(tableExistsResult);
    }
}
