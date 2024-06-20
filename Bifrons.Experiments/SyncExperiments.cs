using Bifrons.Cannonizers.Relational;

namespace Bifrons.Experiments;

[Collection("DatabaseCollection")]
public sealed class SynchronizationExperiments : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public SynchronizationExperiments(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Synchronize()
    {
        var academicCannonizer = _fixture.GetService<ICannonizer>("AcademicCannonizer");
        var financialCannonizer = _fixture.GetService<ICannonizer>("FinancialCannonizer");

        var a_studentsTable = academicCannonizer.MetadataManager.GetTable("Students");
        var f_studentsTable = financialCannonizer.MetadataManager.GetTable("Students");

        Assert.True(a_studentsTable);
        Assert.True(f_studentsTable);
    }
}
