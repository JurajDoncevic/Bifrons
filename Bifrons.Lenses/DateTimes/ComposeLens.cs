namespace Bifrons.Lenses.DateTimes;

/// <summary>
/// Describes a lens that composes two datetime lenses. Is itself an datetime lens.
/// compose(left, right) : DateTime <=> DateTime
public sealed class ComposeLens : SymmetricDateTimeLens
{
    private readonly ComposeLens<DateTime, DateTime, DateTime> _genericComposeLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    private ComposeLens(SymmetricDateTimeLens left, SymmetricDateTimeLens right)
    {
        _genericComposeLens = new ComposeLens<DateTime, DateTime, DateTime>(left, right);
    }

    public override Func<DateTime, Option<DateTime>, Result<DateTime>> PutLeft => _genericComposeLens.PutLeft;
    
    public override Func<DateTime, Option<DateTime>, Result<DateTime>> PutRight => _genericComposeLens.PutRight;

    public override Func<DateTime, Result<DateTime>> CreateRight => _genericComposeLens.CreateRight;

    public override Func<DateTime, Result<DateTime>> CreateLeft => _genericComposeLens.CreateLeft;

    /// <summary>
    /// Constructs a composition lens for datetime lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static ComposeLens Cons(SymmetricDateTimeLens left, SymmetricDateTimeLens right) 
        => new(left, right);
}