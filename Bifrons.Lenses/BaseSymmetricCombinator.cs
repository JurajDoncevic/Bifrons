namespace Bifrons.Lenses;

public abstract class BaseSymmetricCombinator<TLhsLens, TRhsLens, TResLens,
                                              TLhsLensLeft, TLhsLensRight,
                                              TRhsLensLeft, TRhsLensRight,
                                              TResLensLeft, TResLensRight>
    where TLhsLens : BaseSymmetricLens<TLhsLensLeft, TLhsLensRight>
    where TRhsLens : BaseSymmetricLens<TRhsLensLeft, TRhsLensRight>
    where TResLens : BaseSymmetricLens<TResLensLeft, TResLensRight>
{
    public abstract BaseSymmetricLens<TResLensLeft, TResLensRight> Combine(TLhsLens lhsLens, TRhsLens rhsLens);
}
