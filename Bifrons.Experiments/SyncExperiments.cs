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
        var academicCannonizer = _fixture.GetService<ICannonizer>("Academic");
        var financialCannonizer = _fixture.GetService<ICannonizer>("Financial");
    }
}
