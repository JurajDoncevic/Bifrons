
namespace Bifrons.Lenses;

public sealed class InsertLens : BaseAsymmetricLens<string, string>
{
    public override Func<string, Option<string>, Result<string>> Put => throw new NotImplementedException();

    public override Func<string, Result<string>> Get => throw new NotImplementedException();
}
