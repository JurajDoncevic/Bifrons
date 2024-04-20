using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational.Sqlite.Tests;

public sealed class QueryManagerTests
{

    [Fact]
    public void GetFrom_WhenRowExists_ReturnsRowData()
    {
        // Arrange
        var metadataManager = new MetadataManager("Data Source=./PeopleAndRoles.db;");
        var queryManager = new QueryManager("Data Source=./PeopleAndRoles.db");
        var table = metadataManager.GetTable("Person").Data ?? throw new Exception("Table not found");
        var key = ColumnData.Cons(table["Id"].Value, 1).Data;

        // Act
        var result = queryManager.GetFrom(table, key);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data!["Id"].Value.BoxedData as int?);
    }
}
