namespace Bifrons.Lenses.Symmetric.Decimals;

/// <summary>
/// Describes a lens that composes two decimal lenses. Is itself an decimal lens.
/// compose(left, right) : decimal <=> decimal
/// actual decimal type is double for convenience
/// </summary>
public sealed class ComposeLens : SymmetricDecimalLens
{
    private readonly ComposeLens<double, double, double> _genericComposeLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    private ComposeLens(SymmetricDecimalLens left, SymmetricDecimalLens right)
    {
        _genericComposeLens = new ComposeLens<double, double, double>(left, right);
    }

    public override Func<double, Option<double>, Result<double>> PutLeft => _genericComposeLens.PutLeft;

    public override Func<double, Option<double>, Result<double>> PutRight => _genericComposeLens.PutRight;

    public override Func<double, Result<double>> CreateRight => _genericComposeLens.CreateRight;

    public override Func<double, Result<double>> CreateLeft => _genericComposeLens.CreateLeft;

    /// <summary>
    /// Constructs a composition lens for decimal lenses
    /// </summary>
    /// <param name="left">The left lens</param>
    /// <param name="right">The right lens</param>
    public static ComposeLens Cons(SymmetricDecimalLens left, SymmetricDecimalLens right) 
        => new(left, right);

}
