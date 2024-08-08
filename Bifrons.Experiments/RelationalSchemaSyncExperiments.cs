using Bifrons.Cannonizers.Relational;
using Bifrons.Lenses;
using Tables = Bifrons.Lenses.Relational.Tables;
using Columns = Bifrons.Lenses.Relational.Columns;
using Bifrons.Lenses.Relational.Columns;

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

        var a_studentsTable = academicCannonizer.MetadataManager.GetTable("Students").Data ?? throw new Exception("Table not found");
        var f_studentsTable = financialCannonizer.MetadataManager.GetTable("Students").Data ?? throw new Exception("Table not found");
        var a_coursesTable = academicCannonizer.MetadataManager.GetTable("Courses").Data ?? throw new Exception("Table not found");


        var lens_studentId = Columns.IdentityLens.Cons(f_studentsTable["StudentID"].Value.Name);
        var lens_studentName = Columns.IdentityLens.Cons(f_studentsTable["FirstName"].Value.Name);
        var lens_lastName = Columns.IdentityLens.Cons(f_studentsTable["LastName"].Value.Name);
        var lens_email = Columns.IdentityLens.Cons(f_studentsTable["Email"].Value.Name);
        var lens_phoneNumber = Columns.IdentityLens.Cons(f_studentsTable["PhoneNumber"].Value.Name);
        var lens_major = Columns.IdentityLens.Cons(f_studentsTable["Major"].Value.Name);
        var lens_enrollmentDate = Columns.DeleteLens.Cons(a_studentsTable["EnrollmentDate"].Value.Name);
        var columnLenses = Enumerable.AsEnumerable<SymmetricColumnLens>([lens_studentId, lens_studentName, lens_lastName, lens_email, lens_phoneNumber, lens_major, lens_enrollmentDate]);

        var lens_studentsTable = Tables.IdentityLens.Cons(f_studentsTable.Name, columnLenses);


        var result_studentsTable = lens_studentsTable.CreateRight(a_studentsTable); 

        Assert.True(result_studentsTable);
    }
}