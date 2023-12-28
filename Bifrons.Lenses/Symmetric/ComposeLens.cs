namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Composition of two symmetric lenses.
/// </summary>
public class ComposeLens<TLeft, TMid, TRight> : BaseSymmetricLens<TLeft, TRight>
{
    private readonly BaseSymmetricLens<TLeft, TMid> _lhsLens;
    private readonly BaseSymmetricLens<TMid, TRight> _rhsLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    internal ComposeLens(BaseSymmetricLens<TLeft, TMid> lhsLens, BaseSymmetricLens<TMid, TRight> rhsLens)
    {
        _lhsLens = lhsLens;
        _rhsLens = rhsLens;
    }

#pragma warning disable CS8714 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public override Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft =>
        (right, left) => _rhsLens.PutLeft(right, left.Bind(l => _lhsLens.CreateRight(l).ToOption()))
                            .Bind(l => _lhsLens.PutLeft(l, left));

    public override Func<TLeft, Option<TRight>, Result<TRight>> PutRight =>
        (left, right) => _lhsLens.PutRight(left, right.Bind(r => _rhsLens.CreateLeft(r).ToOption()))
                            .Bind(r => _rhsLens.PutRight(r, right));
#pragma warning restore CS8714 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public override Func<TLeft, Result<TRight>> CreateRight =>
        left => _lhsLens.CreateRight(left)
                        .Bind(_rhsLens.CreateRight);

    public override Func<TRight, Result<TLeft>> CreateLeft =>
        right => _rhsLens.CreateLeft(right)
                        .Bind(_lhsLens.CreateLeft);
}

/// <summary>
/// Extension methods for <c>ComposeLens</c>.
/// </summary>
public static class ComposeLens
{
    /// <summary>
    /// Constructs a sequentially composed lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    public static BaseSymmetricLens<TLeft, TRight> Cons<TLeft, TMid, TRight>(this BaseSymmetricLens<TLeft, TMid> lhsLens, BaseSymmetricLens<TMid, TRight> rhsLens)
        => new ComposeLens<TLeft, TMid, TRight>(lhsLens, rhsLens);
}