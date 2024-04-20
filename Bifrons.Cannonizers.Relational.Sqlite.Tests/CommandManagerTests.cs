using Bifrons.Base;
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Cannonizers.Relational.Sqlite.Tests;

public sealed class CommandManagerTests
{

    [Fact]
    public void InsertAndDelete_OnTestTable()
    {
        // Arrange
        var longCol = Column.Cons("LongCOL", DataTypes.LONG);
        var stringCol = Column.Cons("StringCOL", DataTypes.STRING);
        var boolCol = Column.Cons("BoolCOL", DataTypes.BOOLEAN);
        var dateTimeCol = Column.Cons("DateTimeCOL", DataTypes.DATETIME);
        var decimalCol = Column.Cons("DecimalCOL", DataTypes.DECIMAL);
        var table = Table.Cons("TestTable", longCol, stringCol, boolCol, dateTimeCol, decimalCol);

        IEnumerable<RowData> rowData = [
            RowData.Cons([
                ColumnData.Cons(longCol, 1L).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(stringCol, "Test").Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(boolCol, true).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(dateTimeCol, DateTime.Parse("1-1-2024")).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(decimalCol, 1.1).Data ?? throw new Exception("Failed to create column"),
            ]),
            RowData.Cons([
                ColumnData.Cons(longCol, 2L).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(stringCol, "Test").Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(boolCol, true).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(dateTimeCol, DateTime.Parse("2-1-2024")).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(decimalCol, 1.2).Data ?? throw new Exception("Failed to create column"),
            ]),
            RowData.Cons([
                ColumnData.Cons(longCol, 3L).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(stringCol, "Test").Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(boolCol, true).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(dateTimeCol, DateTime.Parse("3-1-2024")).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(decimalCol, 1.3).Data ?? throw new Exception("Failed to create column"),
            ]),
            RowData.Cons([
                ColumnData.Cons(longCol, 4L).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(stringCol, "Test").Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(boolCol, true).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(dateTimeCol, DateTime.Parse("4-1-2024")).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(decimalCol, 1.4).Data ?? throw new Exception("Failed to create column"),
            ]),
            RowData.Cons([
                ColumnData.Cons(longCol, 5L).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(stringCol, "Test").Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(boolCol, true).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(dateTimeCol, DateTime.Parse("5-1-2024")).Data ?? throw new Exception("Failed to create column"),
                ColumnData.Cons(decimalCol, 1.5).Data ?? throw new Exception("Failed to create column"),
            ]),
        ];



        var metadataManager = new MetadataManager("Data Source=PeopleAndRoles.db;");
        var commandManager = new CommandManager("Data Source=PeopleAndRoles.db;");
        var queryManager = new QueryManager("Data Source=PeopleAndRoles.db;");

        if(metadataManager.TableExists(table.Name))
        {
            Assert.True(metadataManager.DropTable(table.Name));
        }
        Assert.True(metadataManager.CreateTable(table));

        // Act
        var insertResult = rowData.Map(rd => commandManager.Insert(table, rd)).Unfold();
        var preQueryResult = queryManager.GetFrom(table, row => row["LongCOL"].Value.BoxedData as long? > 3L);
        var deleteResult = commandManager.Delete(table, row => row["LongCOL"].Value.BoxedData as long? > 3L);
        var postQueryResult = queryManager.GetFrom(table, row => row["LongCOL"].Value.BoxedData as long? > 3L);

        // Assert
        Assert.True(insertResult);
        Assert.True(preQueryResult);
        Assert.NotEmpty(preQueryResult.Data.RowData);
        Assert.True(deleteResult);
        Assert.True(postQueryResult);
        Assert.Empty(postQueryResult.Data.RowData);

        Assert.True(metadataManager.DropTable(table.Name));
    }
}
