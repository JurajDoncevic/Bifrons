using Bifrons.Cannonizers.Relational;
using Tables = Bifrons.Lenses.Relational.Tables;
using Columns = Bifrons.Lenses.Relational.Columns;
using DataColumns = Bifrons.Lenses.RelationalData.Columns;
using DataTables = Bifrons.Lenses.RelationalData.Tables;
using Integers = Bifrons.Lenses.Integers;
using Strings = Bifrons.Lenses.Strings;
using DateTimes = Bifrons.Lenses.DateTimes;
using Booleans = Bifrons.Lenses.Booleans;
using Decimals = Bifrons.Lenses.Decimals;
using Relational = Bifrons.Lenses.Relational;
using RelationalData = Bifrons.Lenses.RelationalData;

namespace Bifrons.Experiments;

[Collection("DatabaseCollection")]
public class RelationalSchemaSyncExperiments : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public RelationalSchemaSyncExperiments(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Synchronize_StudentsTable()
    {
        // left
        var academicCannonizer = _fixture.GetService<ICannonizer>("AcademicCannonizer");
        // right
        var financialCannonizer = _fixture.GetService<ICannonizer>("FinancialCannonizer");

        var a_studentsTable = academicCannonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        var f_studentsTable = financialCannonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        var a_coursesTable = academicCannonizer.MetadataManager.GetTable("Courses")
            .Match(
                table => table,
                error => throw new Exception(error)
            );


        var lens_studentId = Columns.IdentityLens.Cons("StudentID");
        var lens_studentName = Columns.IdentityLens.Cons("FirstName");
        var lens_lastName = Columns.IdentityLens.Cons("LastName");
        var lens_email = Columns.IdentityLens.Cons("Email");
        var lens_phoneNumber = Columns.IdentityLens.Cons("PhoneNumber");
        var lens_major = Columns.IdentityLens.Cons("Major");
        var lens_enrollmentDate = Columns.DeleteLens.Cons("EnrollmentDate");
        var columnLenses = Enumerable.AsEnumerable<Relational.Columns.SymmetricColumnLens>([lens_studentId, lens_studentName, lens_lastName, lens_email, lens_phoneNumber, lens_major, lens_enrollmentDate]);

        var lens_studentsTable = Tables.IdentityLens.Cons(f_studentsTable.Name, columnLenses);


        var result_studentsTable = lens_studentsTable.CreateRight(a_studentsTable);

        Assert.True(result_studentsTable);
    }

    [Fact]
    public void SynchronizeRight_LectureRoomsTable_WhenCreatedLeft()
    {
        // ARRANGE
        // setup cannonizers
        // left
        var academicCannonizer = _fixture.GetService<ICannonizer>("AcademicCannonizer");
        // right
        var financialCannonizer = _fixture.GetService<ICannonizer>("FinancialCannonizer");

        // create LectureRoom table (model)
        var lectureRoomsTable = Bifrons.Lenses.Relational.Model.Table.Cons(
            "LectureRooms",
            new List<Bifrons.Lenses.Relational.Model.Column> {
                Bifrons.Lenses.Relational.Model.Column.Cons("RoomID", Bifrons.Lenses.Relational.Model.DataTypes.INTEGER),
                Bifrons.Lenses.Relational.Model.Column.Cons("RoomName", Bifrons.Lenses.Relational.Model.DataTypes.STRING),
                Bifrons.Lenses.Relational.Model.Column.Cons("Capacity", Bifrons.Lenses.Relational.Model.DataTypes.INTEGER),
                Bifrons.Lenses.Relational.Model.Column.Cons("Building", Bifrons.Lenses.Relational.Model.DataTypes.STRING),
                Bifrons.Lenses.Relational.Model.Column.Cons("Floor", Bifrons.Lenses.Relational.Model.DataTypes.INTEGER)
        });

        // create LectureRooms table in Academic database
        var a_lectureRoomsTableCreation = academicCannonizer.MetadataManager.CreateTable(lectureRoomsTable)
            .Match(
                _ => _,
                error => throw new Exception(error)
            );

        // ACT
        // get LectureRooms table from Academic database
        var a_lectureRoomsTable = academicCannonizer.MetadataManager.GetTable(lectureRoomsTable.Name)
            .Match(
                table => table,
                error => throw new Exception(error)
            );

        // create identity lens for LectureRooms table
        var lens_roomId = Columns.IdentityLens.Cons("RoomID");
        var lens_roomName = Columns.IdentityLens.Cons("RoomName");
        var lens_capacity = Columns.IdentityLens.Cons("Capacity");
        var lens_building = Columns.IdentityLens.Cons("Building");
        var lens_floor = Columns.IdentityLens.Cons("Floor");
        var columnLenses = Enumerable.AsEnumerable<Relational.Columns.SymmetricColumnLens>([lens_roomId, lens_roomName, lens_capacity, lens_building, lens_floor]);
        var lens_lectureRoomsTable = Tables.IdentityLens.Cons(lectureRoomsTable.Name, columnLenses);

        // create LectureRooms table in Financial database
        var f_lectureRoomsTableCreation = lens_lectureRoomsTable.CreateRight(a_lectureRoomsTable)
            .Bind(tbl => financialCannonizer.MetadataManager.CreateTable(tbl));
        // get LectureRooms table from Financial database
        var f_lectureRoomsTable = financialCannonizer.MetadataManager.GetTable(lectureRoomsTable.Name)
            .Match(
                table => table,
                error => throw new Exception(error)
            );

        // ASSERT
        Assert.Equal(a_lectureRoomsTable, f_lectureRoomsTable);

        // CLEANUP
        // drop LectureRooms table from Academic database
        var a_lectureRoomsTableDeletion = academicCannonizer.MetadataManager.DropTable(lectureRoomsTable.Name)
            .Match(
                _ => _,
                error => throw new Exception(error)
            );

        // drop LectureRooms table from Financial database
        var f_lectureRoomsTableDeletion = financialCannonizer.MetadataManager.DropTable(lectureRoomsTable.Name)
            .Match(
                _ => _,
                error => throw new Exception(error)
            );
    }
}