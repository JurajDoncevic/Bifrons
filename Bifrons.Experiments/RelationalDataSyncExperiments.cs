using Bifrons.Canonizers.Relational;
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

[Collection("Database collection")]
public sealed class RelationalDataSyncExperiments
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
        // setup canonizers
        // left
        var academicCanonizer = _fixture.GetService<ICanonizer>("AcademicCanonizer");
        // right
        var financialCanonizer = _fixture.GetService<ICanonizer>("FinancialCanonizer");

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
        var tblLeft_students = academicCanonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        var tblDataLeft_students = academicCanonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        // load right student data
        var tblRight_students = financialCanonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        var tblDataRight_students = financialCanonizer.QueryManager.GetAllFrom(tblRight_students)
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
        // setup canonizers
        // left
        var academicCanonizer = _fixture.GetService<ICanonizer>("AcademicCanonizer");
        // right
        var financialCanonizer = _fixture.GetService<ICanonizer>("FinancialCanonizer");

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
        var tblLeft_students = academicCanonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        // load right student table model
        var tblRight_students = financialCanonizer.MetadataManager.GetTable("Students")
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
            (rowData, acc) => acc.Append(academicCanonizer.CommandManager.Insert(tblLeft_students, rowData))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );

        // load left student data
        var tblDataLeft_students = academicCanonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        // load right student data
        var tblDataRight_students = financialCanonizer.QueryManager.GetAllFrom(tblRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        #endregion

        // ACT
        var res_tblData_rightStudents = tblDataLens_students.PutRight(tblDataLeft_students, tblDataRight_students);

        var res_financialDataSync = res_tblData_rightStudents
            .Bind(students => financialCanonizer.CommandManager.SyncIntoDatabase(students.Table, new List<Column> { tblRight_students["StudentID"].Value }, students.RowData));

        var rightTblData_students = financialCanonizer.QueryManager.GetAllFrom(tblRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );


        // ASSERT
        Assert.True(res_tblData_rightStudents);
        Assert.True(res_financialDataSync);
        Assert.Equal(tblDataLeft_students.RowData.Count, rightTblData_students.RowData.Count);


        // CLEANUP
        // delete data from left/academic database
        var res_academicDeletion = newAcademicStudentData.Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(academicCanonizer.CommandManager.Delete(tblLeft_students, rd => rd["StudentID"] == rowData["StudentID"]))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );
        var res_financialDeletion = newAcademicStudentData.Fold( // doesn't matter if we delete from financial or academic database, since we only use IDs
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(financialCanonizer.CommandManager.Delete(tblRight_students, rd => rd["StudentID"] == rowData["StudentID"]))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );

    }

    [Fact]
    public void Synchronize_StudentTableData_WhenRightDataIsAdded()
    {
        #region ARRANGE
        // ARRANGE
        // setup canonizers
        // left
        var academicCanonizer = _fixture.GetService<ICanonizer>("AcademicCanonizer");
        // right
        var financialCanonizer = _fixture.GetService<ICanonizer>("FinancialCanonizer");

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
        var tblLeft_students = academicCanonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        // load right student table model
        var tblRight_students = financialCanonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );

        // insert new data to right/financial database
        var newFinancialStudentData = Enumerable.AsEnumerable(
        [
            RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblRight_students["StudentID"].Value, "QQQQQQQQQQ").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["FirstName"].Value, "Tiff").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["LastName"].Value, "Shaw").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["Email"].Value, "tiff.shaw@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["PhoneNumber"].Value, "123-456-7777").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["BillingAddress"].Value, "123 Village St, City, State, ZIP").Match(_ => _, error => throw new Exception(error))
                    }),
            RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblRight_students["StudentID"].Value, "PPPPPPPPPP").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["FirstName"].Value, "Tim").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["LastName"].Value, "Needell").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["Email"].Value, "tim.needell@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["PhoneNumber"].Value, "765-432-1111").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["BillingAddress"].Value, "456 Village St, City, State, ZIP").Match(_ => _, error => throw new Exception(error))
                    })
        ]);

        var res_financialInsertion = newFinancialStudentData.Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(financialCanonizer.CommandManager.Insert(tblRight_students, rowData))
        ).Unfold();

        // load left student data
        var tblDataLeft_students = academicCanonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        // load right student data
        var tblDataRight_students = financialCanonizer.QueryManager.GetAllFrom(tblRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        #endregion

        // ACT
        var res_tblData_leftStudents = tblDataLens_students.PutLeft(tblDataRight_students, tblDataLeft_students);

        var res_academicDataSync = res_tblData_leftStudents
            .Bind(students => academicCanonizer.CommandManager.SyncIntoDatabase(students.Table, new List<Column> { tblLeft_students["StudentID"].Value }, students.RowData));

        var leftTblData_students = academicCanonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );

        // ASSERT
        Assert.True(res_tblData_leftStudents);
        Assert.True(res_academicDataSync);
        Assert.Equal(leftTblData_students.RowData.Count, tblDataRight_students.RowData.Count);

        // CLEANUP
        // delete data from left/academic database
        var res_academicDeletion = newFinancialStudentData.Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(academicCanonizer.CommandManager.Delete(tblLeft_students, rd => rd["StudentID"] == rowData["StudentID"]))
        ).Unfold();

        // delete data from right/financial database
        var res_financialDeletion = newFinancialStudentData.Fold( // doesn't matter if we delete from financial or academic database, since we only use IDs
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(financialCanonizer.CommandManager.Delete(tblRight_students, rd => rd["StudentID"] == rowData["StudentID"]))
        ).Unfold();
    }

    [Fact]
    public void Synchronize_StudentTableData_WhenLeftAndRightDataIsAdded()
    {
        #region ARRANGE
        // ARRANGE
        // setup canonizers
        // left
        var academicCanonizer = _fixture.GetService<ICanonizer>("AcademicCanonizer");
        // right
        var financialCanonizer = _fixture.GetService<ICanonizer>("FinancialCanonizer");

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
        var tblLeft_students = academicCanonizer.MetadataManager.GetTable("Students")
            .Match(
                table => table,
                error => throw new Exception(error)
            );
        // load right student table model
        var tblRight_students = financialCanonizer.MetadataManager.GetTable("Students")
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
                        ColumnData.Cons(tblLeft_students["StudentID"].Value, "XXXXXXXXXX").Match(_ => _, error => throw new Exception(error)),
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
                        ColumnData.Cons(tblLeft_students["StudentID"].Value, "YYYYYYYYYY").Match(_ => _, error => throw new Exception(error)),
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
                        ColumnData.Cons(tblLeft_students["StudentID"].Value, "ZZZZZZZZZZ").Match(_ => _, error => throw new Exception(error)),
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
            (rowData, acc) => acc.Append(academicCanonizer.CommandManager.Insert(tblLeft_students, rowData))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );

        // insert new data to right/financial database
        var newFinancialStudentData = Enumerable.AsEnumerable(
        [
            RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblRight_students["StudentID"].Value, "QQQQQQQQQQ").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["FirstName"].Value, "Tiff").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["LastName"].Value, "Shaw").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["Email"].Value, "tiff.shaw@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["PhoneNumber"].Value, "123-456-7777").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["BillingAddress"].Value, "123 Village St, City, State, ZIP").Match(_ => _, error => throw new Exception(error))
                    }),
            RowData.Cons(
                    new List<ColumnData>
                    {
                        ColumnData.Cons(tblRight_students["StudentID"].Value, "PPPPPPPPPP").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["FirstName"].Value, "Tim").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["LastName"].Value, "Needell").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["Email"].Value, "tim.needell@example.com").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["PhoneNumber"].Value, "765-432-1111").Match(_ => _, error => throw new Exception(error)),
                        ColumnData.Cons(tblRight_students["BillingAddress"].Value, "456 Village St, City, State, ZIP").Match(_ => _, error => throw new Exception(error))
                    })
        ]);

        var res_financialInsertion = newFinancialStudentData.Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(financialCanonizer.CommandManager.Insert(tblRight_students, rowData))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );

        // load left student data
        var tblDataLeft_students = academicCanonizer.QueryManager.GetAllFrom(tblLeft_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        // load right student data
        var tblDataRight_students = financialCanonizer.QueryManager.GetAllFrom(tblRight_students)
            .Match(
                data => data,
                error => throw new Exception(error)
            );
        #endregion


        #region ACT
        // ACT
        // sync data right then left
        var res_studentsDataSync = tblDataLens_students.PutRight(tblDataLeft_students, tblDataRight_students)
            .Bind(rightData => tblDataLens_students.PutLeft(rightData, tblDataLeft_students).Map(leftData => (leftData, rightData)));

        // use the canonizers to place the data into the databases
        var res_databaseSync = res_studentsDataSync.Bind(data => academicCanonizer.CommandManager.SyncIntoDatabase(tblLeft_students, new List<Column> { tblLeft_students["StudentID"].Value }, data.Item1.RowData))
            .Bind(_ => financialCanonizer.CommandManager.SyncIntoDatabase(tblRight_students, new List<Column> { tblRight_students["StudentID"].Value }, res_studentsDataSync.Data.Item2.RowData));

        // get the synced data from the databases
        var res_syncedAcademicData = academicCanonizer.QueryManager.GetAllFrom(tblLeft_students);
        var res_syncedFinancialData = financialCanonizer.QueryManager.GetAllFrom(tblRight_students);

        #endregion

        #region ASSERT
        // ASSERT
        Assert.True(res_studentsDataSync);
        var (leftData, rightData) = res_studentsDataSync.Data;
        Assert.Equal(leftData.RowData.Count, rightData.RowData.Count);

        Assert.True(res_databaseSync);
        Assert.True(res_syncedAcademicData);
        Assert.True(res_syncedFinancialData);
        Assert.Equal(res_syncedAcademicData.Data.RowData.Count, res_syncedFinancialData.Data.RowData.Count);

        #endregion

        #region CLEANUP
        // CLEANUP
        // delete data from left/academic database
        var res_academicDeletion = newAcademicStudentData.Concat(newFinancialStudentData).Fold(
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(academicCanonizer.CommandManager.Delete(tblLeft_students, rd => rd["StudentID"].Equals(rowData["StudentID"])))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );
        // delete data from right/financial database
        var res_financialDeletion = newFinancialStudentData.Concat(newAcademicStudentData).Fold( // doesn't matter if we delete from financial or academic database, since we only use IDs
            Enumerable.Empty<Result<Unit>>(),
            (rowData, acc) => acc.Append(financialCanonizer.CommandManager.Delete(tblRight_students, rd => rd["StudentID"].Equals(rowData["StudentID"])))
        ).Unfold()
        .Match(
            data => data,
            error => throw new Exception(error)
        );
        #endregion
    }
}
