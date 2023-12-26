namespace Bifrons.Lenses.Symmetric.Strings;

public abstract class SymmetricStringLens : BaseSymmetricLens<string, string>
{
    public static SymmetricStringLens operator |(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => Combinators.Concat(lhsLens, rhsLens);
}
