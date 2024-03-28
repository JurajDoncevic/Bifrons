

namespace Bifrons.Lenses.Booleans;

public abstract class SymmetricBoolLens : ISymmetricLens<bool, bool>
{
    public abstract Func<bool, Option<bool>, Result<bool>> PutLeft { get; }

    public abstract Func<bool, Option<bool>, Result<bool>> PutRight { get; }

    public abstract Func<bool, Result<bool>> CreateRight { get; }

    public abstract Func<bool, Result<bool>> CreateLeft { get; }
}
