namespace Bifrons.Lenses.Symmetric.Integers;

public sealed class ComposeLens : SymmetricIntegerLens
{
    private readonly ComposeLens<int, int, int> _genericComposeLens;

    private ComposeLens(SymmetricIntegerLens left, SymmetricIntegerLens right)
    {
        _genericComposeLens = new ComposeLens<int, int, int>(left, right);
    }

    public override Func<int, Option<int>, Result<int>> PutLeft => _genericComposeLens.PutLeft;
    
    public override Func<int, Option<int>, Result<int>> PutRight => _genericComposeLens.PutRight;

    public override Func<int, Result<int>> CreateRight => _genericComposeLens.CreateRight;

    public override Func<int, Result<int>> CreateLeft => _genericComposeLens.CreateLeft;

    public static ComposeLens Cons(SymmetricIntegerLens left, SymmetricIntegerLens right) 
        => new(left, right);
}
