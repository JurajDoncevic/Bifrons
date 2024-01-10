namespace Bifrons.Lenses.Symmetric.Integers;

public abstract class SymmetricIntegerLens : BaseSymmetricLens<int, int>
{
    public static SymmetricIntegerLens operator>>(SymmetricIntegerLens left, SymmetricIntegerLens right) 
        => ComposeLens.Cons(left, right);
}
