namespace Bifrons.Lenses.Longs;

/// <summary>
/// Describes a lens that composes two long lenses. Is itself an long lens.
/// compose(left, right) : long <=> long
public sealed class ComposeLens : SymmetricLongLens
{
    private readonly ComposeLens<long, long, long> _genericComposeLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    private ComposeLens(SymmetricLongLens left, SymmetricLongLens right)
    {
        _genericComposeLens = new ComposeLens<long, long, long>(left, right);
    }

    public override Func<long, Option<long>, Result<long>> PutLeft => _genericComposeLens.PutLeft;
    
    public override Func<long, Option<long>, Result<long>> PutRight => _genericComposeLens.PutRight;

    public override Func<long, Result<long>> CreateRight => _genericComposeLens.CreateRight;

    public override Func<long, Result<long>> CreateLeft => _genericComposeLens.CreateLeft;

    /// <summary>
    /// Constructs a composition lens for long lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static ComposeLens Cons(SymmetricLongLens left, SymmetricLongLens right) 
        => new(left, right);
}
