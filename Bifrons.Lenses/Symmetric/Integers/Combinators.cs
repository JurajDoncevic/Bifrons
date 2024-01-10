namespace Bifrons.Lenses.Symmetric.Integers;

public static class Combinators
{
    public static SymmetricIntegerLens Compose(SymmetricIntegerLens left, SymmetricIntegerLens right)
        => ComposeLens.Cons(left, right);
}
