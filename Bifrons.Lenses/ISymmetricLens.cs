namespace Bifrons.Lenses;

/// <summary>
/// Abstractly describes a SIMPLE symmetric lens. L : R <=> L
/// </summary>
/// <typeparam name="TLeft">Left type</typeparam>
/// <typeparam name="TRight">Right type</typeparam>
public interface ISymmetricLens<TLeft, TRight>
{
    /// <summary>
    /// <c>putL : Y -> Option X -> Result X</c>
    /// </summary>
    public abstract Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft { get; }
    /// <summary>
    /// <c>putR : X -> Option Y -> Result Y</c>
    /// </summary>
    public abstract Func<TLeft, Option<TRight>, Result<TRight>> PutRight { get; }
    /// <summary>
    /// <c>createR : X -> Result Y</c>
    /// </summary>
    public abstract Func<TLeft, Result<TRight>> CreateRight { get; }
    /// <summary>
    /// <c>createL : Y -> Result X</c>
    /// </summary>
    public abstract Func<TRight, Result<TLeft>> CreateLeft { get; }

    /// <summary>
    /// Inverts the lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static InvertLens<TRight, TLeft> operator !(ISymmetricLens<TLeft, TRight> originalLens)
        => InvertLens.Cons(originalLens);

}