namespace Bifrons.Lenses.Symmetric.Integers;

/// <summary>
/// Describes a lens that composes two integer lenses. Is itself an integer lens.
/// compose(left, right) : int <=> int
public sealed class ComposeLens : SymmetricIntegerLens
{
    private readonly ComposeLens<int, int, int> _genericComposeLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    private ComposeLens(SymmetricIntegerLens left, SymmetricIntegerLens right)
    {
        _genericComposeLens = new ComposeLens<int, int, int>(left, right);
    }

    public override Func<int, Option<int>, Result<int>> PutLeft => _genericComposeLens.PutLeft;
    
    public override Func<int, Option<int>, Result<int>> PutRight => _genericComposeLens.PutRight;

    public override Func<int, Result<int>> CreateRight => _genericComposeLens.CreateRight;

    public override Func<int, Result<int>> CreateLeft => _genericComposeLens.CreateLeft;

    /// <summary>
    /// Constructs a composition lens for integer lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static ComposeLens Cons(SymmetricIntegerLens left, SymmetricIntegerLens right) 
        => new(left, right);
}
