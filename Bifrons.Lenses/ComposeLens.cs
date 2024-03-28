namespace Bifrons.Lenses;

/// <summary>
/// Composition of two symmetric lenses.
/// </summary>
public class ComposeLens<TLeft, TMid, TRight> : ISymmetricLens<TLeft, TRight>
{
    private readonly ISymmetricLens<TLeft, TMid> _lhsLens;
    private readonly ISymmetricLens<TMid, TRight> _rhsLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="lhsLens">Left-hand side operand lens</param>
    /// <param name="rhsLens">Right-hand side operand lens</param>
    internal ComposeLens(ISymmetricLens<TLeft, TMid> lhsLens, ISymmetricLens<TMid, TRight> rhsLens)
    {
        _lhsLens = lhsLens;
        _rhsLens = rhsLens;
    }

#pragma warning disable CS8714 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft =>
        (updatedSource, originalTarget) => _rhsLens.PutLeft(updatedSource, originalTarget.Bind(l => _lhsLens.CreateRight(l).ToOption()))
                            .Bind(l => _lhsLens.PutLeft(l, originalTarget));

    public Func<TLeft, Option<TRight>, Result<TRight>> PutRight =>
        (updatedSource, originalTarget) => _lhsLens.PutRight(updatedSource, originalTarget.Bind(r => _rhsLens.CreateLeft(r).ToOption()))
                            .Bind(r => _rhsLens.PutRight(r, originalTarget));
#pragma warning restore CS8714 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public Func<TLeft, Result<TRight>> CreateRight =>
        source => _lhsLens.CreateRight(source)
                        .Bind(_rhsLens.CreateRight);

    public Func<TRight, Result<TLeft>> CreateLeft =>
        source => _rhsLens.CreateLeft(source)
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
    public static ISymmetricLens<TLeft, TRight> Cons<TLeft, TMid, TRight>(this ISymmetricLens<TLeft, TMid> lhsLens, ISymmetricLens<TMid, TRight> rhsLens)
        => new ComposeLens<TLeft, TMid, TRight>(lhsLens, rhsLens);
}