namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Describes an invert lens. L : R <=> L
/// </summary>
public class InvertLens<TLeft, TRight> : BaseSymmetricLens<TLeft, TRight>
{
    private readonly BaseSymmetricLens<TRight, TLeft> _originalLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    internal InvertLens(BaseSymmetricLens<TRight, TLeft> originalLens)
    {
        _originalLens = originalLens;
    }

    public override Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft => _originalLens.PutRight;

    public override Func<TLeft, Option<TRight>, Result<TRight>> PutRight => _originalLens.PutLeft;

    public override Func<TLeft, Result<TRight>> CreateRight => _originalLens.CreateLeft;

    public override Func<TRight, Result<TLeft>> CreateLeft => _originalLens.CreateRight;
}

/// <summary>
/// Static class for creating invert lenses
/// </summary>
public static class InvertLens
{
    /// <summary>
    /// Constructs an invert lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static InvertLens<TRight, TLeft> Cons<TRight, TLeft>(BaseSymmetricLens<TLeft, TRight> originalLens)
        => new(originalLens);
}