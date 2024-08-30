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
using Bifrons.Lenses.Relational.Model;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Experiments;

[Collection("DatabaseCollection")]
public sealed class RelationalDataSyncExperiments : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public RelationalDataSyncExperiments(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Synchronize_StudentTableData_WhenNoDataIsAdded()
    {
        #region ARRANGE
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
            },
            new List<Column>() { Column.Cons("StudentID", Relational.Model.DataTypes.STRING) }
            ).Match(
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
        #endregion

        // ACT
        // get data
        var tblData_rightStudents = tblDataLens_students.CreateRight(tblDataLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        var tblData_leftStudents = tblDataLens_students.CreateLeft(tblDataRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );

        //sync data - no new data to sync

        var res_rightDataSync = tblDataLens_students.PutRight(tblData_leftStudents, tblData_rightStudents);

        // place data

        // ASSERT
        Assert.True(res_rightDataSync);
        Assert.Equal(tblDataLeft_students.RowData.Count, tblDataRight_students.RowData.Count);
        Assert.Equal(res_rightDataSync.Data, tblData_rightStudents);
    }

    [Fact]
    public void Synchronize_StudentTableData_WhenLeftDataIsAdded()
    {
        #region ARRANGE
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
            },
            new List<Column>() { Column.Cons("StudentID", Relational.Model.DataTypes.STRING) }
            ).Match(
                lens => lens,
                error => throw new Exception(error)
            );

        // load left student table model
        var tblLeft_students = academicCannonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        // load right student table model
        var tblRight_students = financialCannonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );

        // insert new data to left/academic database
        var newAcademicStudentData = Enumerable.AsEnumerable(
            [
                RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblLeft_students["StudentID"].Value, "XXXXXX").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["FirstName"].Value, "James").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["LastName"].Value, "Clarkson").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["Email"].Value, "james.clarkson@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["PhoneNumber"].Value, "666-666-6666").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["Major"].Value, "Transportation").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["EnrollmentDate"].Value, DateTime.MinValue).Match(_ => _, error => throw new Exception(error)),
                    }),
                RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblLeft_students["StudentID"].Value, "YYYYYY").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["FirstName"].Value, "Jeremy").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["LastName"].Value, "Hammond").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["Email"].Value, "jeremy.dammond@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["PhoneNumber"].Value, "777-777-7777").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["Major"].Value, "Machine engineering").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["EnrollmentDate"].Value, DateTime.MinValue).Match(_ => _, error => throw new Exception(error)),
                    }),
                RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblLeft_students["StudentID"].Value, "ZZZZZZ").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["FirstName"].Value, "Richard").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["LastName"].Value, "May").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["Email"].Value, "richard.may@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["PhoneNumber"].Value, "888-888-8888").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["Major"].Value, "Automotive engineering").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblLeft_students["EnrollmentDate"].Value, DateTime.MinValue).Match(_ => _, error => throw new Exception(error)),
                    }),
            ]);

        var res_academicInsertion = newAcademicStudentData.Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(academicCannonizer.CommandManager.Insert(tblLeft_students, rowData))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );

        // load left student data
        var tblDataLeft_students = academicCannonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        // load right student data
        var tblDataRight_students = financialCannonizer.QueryManager.GetAllFrom(tblRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        #endregion



        // ACT
        var res_tblData_rightStudents = tblDataLens_students.PutRight(tblDataLeft_students, tblDataRight_students);

        var res_tblData_leftStudents = tblDataLens_students.PutLeft(tblDataRight_students, tblDataLeft_students);

        // ASSERT
        Assert.True(res_tblData_rightStudents);
        Assert.Equal(tblDataLeft_students.RowData.Count, tblDataRight_students.RowData.Count);

        Assert.True(res_tblData_leftStudents);
        Assert.Equal(tblDataLeft_students.RowData.Count, tblDataRight_students.RowData.Count);

        // CLEANUP
        // delete data from left/academic database
        var res_academicDeletion = newAcademicStudentData.Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(academicCannonizer.CommandManager.Delete(tblLeft_students, rd => rd["StudentID"] == rowData["StudentID"]))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );
        var res_financialDeletion = newAcademicStudentData.Fold( // doesn't matter if we delete from financial or academic database, since we only use IDs
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(financialCannonizer.CommandManager.Delete(tblRight_students, rd => rd["StudentID"] == rowData["StudentID"]))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );

    }
}
