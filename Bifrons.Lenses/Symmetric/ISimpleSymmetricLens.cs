namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Abstractly describes a SIMPLE symmetric lens. L : R <=> L
/// </summary>
/// <typeparam name="TLeft">Left type</typeparam>
/// <typeparam name="TRight">Right type</typeparam>
public interface ISimpleSymmetricLens<TLeft, TRight>
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
    /// <c>createR : Option X -> Result Y</c>
    /// </summary>
    public abstract Func<TLeft, Result<TRight>> CreateRight { get; }
    /// <summary>
    /// <c>createL : Option Y -> Result X</c>
    /// </summary>
    public abstract Func<TRight, Result<TLeft>> CreateLeft { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    //protected IBaseSymmetricLens() { }

    /// <summary>
    /// Inverts the lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static InvertLens<TRight, TLeft> operator !(ISimpleSymmetricLens<TLeft, TRight> originalLens)
        => InvertLens.Cons(originalLens);

}