using Bifrons.Lenses.Relational.Model;

namespace Bifrons.Cannonizers.Relational.Sqlite.Tests;

public class MetadataManagerTests
{
    private readonly MetadataManager _metadataManager;

    public MetadataManagerTests()
    {
        _metadataManager = new MetadataManager("Data Source=./PeopleAndRoles.db;");
    }

    [Fact(DisplayName = "Get metadata for a table")]
    public void GetTableMetadata()
    {
        var tableResult = _metadataManager.GetTable("Person");
        var table = tableResult.Data;
        Assert.True(tableResult);
        Assert.Equal("Person", table.Name);
        Assert.Equal(4, table.Columns.Count);
        Assert.Equal("Id", table.Columns[0].Name);
        Assert.Equal(DataTypes.INTEGER, table.Columns[0].DataType);
        Assert.Equal("FirstName", table.Columns[1].Name);
        Assert.Equal(DataTypes.STRING, table.Columns[1].DataType);
        Assert.Equal("LastName", table.Columns[2].Name);
        Assert.Equal(DataTypes.STRING, table.Columns[2].DataType);
        Assert.Equal("DateOfBirth", table.Columns[3].Name);
        Assert.Equal(DataTypes.DATETIME, table.Columns[3].DataType);
    }
}
