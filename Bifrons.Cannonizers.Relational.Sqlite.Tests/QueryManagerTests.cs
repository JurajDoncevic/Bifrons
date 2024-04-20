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
        var key = ColumnData.Cons(table["Id"].Value, 1L).Data;

        // Act
        var result = queryManager.GetFrom(table, key);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(key.BoxedData, result.Data.RowData.FirstOrDefault()?["Id"].Value.BoxedData);
    }

    [Fact]
    public void GetFrom_WhenRowDoesNotExist_ReturnsEmpty()
    {
        // Arrange
        var metadataManager = new MetadataManager("Data Source=./PeopleAndRoles.db;");
        var queryManager = new QueryManager("Data Source=./PeopleAndRoles.db");
        var table = metadataManager.GetTable("Person").Data ?? throw new Exception("Table not found");
        var key = ColumnData.Cons(table["Id"].Value, 100L).Data;

        // Act
        var result = queryManager.GetFrom(table, key);

        // Assert
        Assert.True(result);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.RowData);
    }

    [Fact]
    public void GetAllFrom_WhenTableHasRows_ReturnsTableData()
    {
        // Arrange
        var metadataManager = new MetadataManager("Data Source=./PeopleAndRoles.db;");
        var queryManager = new QueryManager("Data Source=./PeopleAndRoles.db");
        var table = metadataManager.GetTable("Person").Data ?? throw new Exception("Table not found");

        // Act
        var result = queryManager.GetAllFrom(table);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data!.RowData);
    }

    [Fact]
    public void GetFrom_WithPredicate_WhenTableHasData_TableData()
    {
        // Arrange
        var metadataManager = new MetadataManager("Data Source=./PeopleAndRoles.db;");
        var queryManager = new QueryManager("Data Source=./PeopleAndRoles.db");
        var table = metadataManager.GetTable("Person").Data ?? throw new Exception("Table not found");
        var predicate = new Func<RowData, bool>(row => row["FirstName"].Value.BoxedData as string == "John" && row["LastName"].Value.BoxedData as string == "Smith");

        // Act
        var result = queryManager.GetAllFrom(table);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data!.RowData);
    }
}
