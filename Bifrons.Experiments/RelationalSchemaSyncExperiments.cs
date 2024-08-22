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

    [Fact]
    public void Synchronize_StudentTableData_WhenLeftAndRightAreAdded() 
    {
        // ARRANGE
        // setup cannonizers
        // left
        var academicCannonizer = _fixture.GetService<ICannonizer>("AcademicCannonizer");
        // right
        var financialCannonizer = _fixture.GetService<ICannonizer>("FinancialCannonizer");

        // create student table structure lens
        var colLens_studentId = Columns.IdentityLens.Cons("StudentID");
        var colLens_firstName = Columns.IdentityLens.Cons("FirstName");
        var colLens_lastName = Columns.IdentityLens.Cons("LastName");
        var colLens_email = Columns.IdentityLens.Cons("Email");
        var colLens_phoneNumber = Columns.IdentityLens.Cons("PhoneNumber");
        var colLens_major = Columns.DeleteLens.Cons("Major", Relational.Model.DataTypes.STRING);
        var colLens_enrollmentDate = Columns.DeleteLens.Cons("EnrollmentDate", Relational.Model.DataTypes.DATETIME);
        var colLens_billingAddress = Columns.InsertLens.Cons("BillingAddress", Relational.Model.DataTypes.STRING);
        var tblLens_students = Tables.IdentityLens.Cons(
            "Students", 
            new List<Relational.Columns.SymmetricColumnLens>
            {
                colLens_studentId,
                colLens_firstName,
                colLens_lastName,
                colLens_email,
                colLens_phoneNumber,
                colLens_major,
                colLens_enrollmentDate,
                colLens_billingAddress
            });

        // create student table data lens
        var colDataLens_studentId = DataColumns.StringIdentityLens.Cons(
            colLens_studentId,
            Strings.IdentityLens.Cons()
            );
        var colDataLens_firstName = DataColumns.StringIdentityLens.Cons(
            colLens_firstName,
            Strings.IdentityLens.Cons()
            );
        var colDataLens_lastName = DataColumns.StringIdentityLens.Cons(
            colLens_lastName,
            Strings.IdentityLens.Cons()
            );
        var colDataLens_email = DataColumns.StringIdentityLens.Cons(
            colLens_email,
            Strings.IdentityLens.Cons()
            );
        var colDataLens_phoneNumber = DataColumns.StringIdentityLens.Cons(
            colLens_phoneNumber,
            Strings.IdentityLens.Cons()
            );
        var colDataLens_major = DataColumns.StringDeleteLens.Cons(
            colLens_major,
            "N/A"
            );
        var colDataLens_enrollmentDate = DataColumns.DateTimeDeleteLens.Cons(
            colLens_enrollmentDate,
            DateTime.MinValue
            );
        var colDataLens_billingAddress = DataColumns.StringInsertLens.Cons(
            colLens_billingAddress,
            "N/A"
            );
        var tblDataLens_students = DataTables.IdentityLens.Cons(
            tblLens_students,
            new List<RelationalData.Columns.ISymmetricColumnDataLens>
            {
                colDataLens_studentId,
                colDataLens_firstName,
                colDataLens_lastName,
                colDataLens_email,
                colDataLens_phoneNumber,
                colDataLens_major,
                colDataLens_enrollmentDate,
                colDataLens_billingAddress
            }).Match(
                lens => lens,
                error => throw new Exception(error)
            );

        // load left student data
        var tblLeft_students = academicCannonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        var tblDataLeft_students = academicCannonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        // load right student data
        var tblRight_students = financialCannonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        var tblDataRight_students = financialCannonizer.QueryManager.GetAllFrom(tblRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );

        // ACT
        // get data
        var res_tblData_rightStudents = tblDataLens_students.CreateRight(tblDataLeft_students);
        var res_tblData_leftStudents = tblDataLens_students.CreateLeft(tblDataRight_students);

        //sync data

        // place data

        // ASSERT
        Assert.True(res_tblData_rightStudents);
        Assert.True(res_tblData_leftStudents);
    }
}