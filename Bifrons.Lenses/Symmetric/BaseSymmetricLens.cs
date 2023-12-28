namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Abstractly describes a SIMPLE symmetric lens. L : R <=> L
/// </summary>
/// <typeparam name="TLeft">Left type</typeparam>
/// <typeparam name="TRight">Right type</typeparam>
public abstract class BaseSymmetricLens<TLeft, TRight>
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
    protected BaseSymmetricLens() { }

    /// <summary>
    /// Calls <c>PutL</c>. See <see cref="PutLeft"/>
    /// </summary>
    public Result<TLeft> CallPutLeft(TRight right, Option<TLeft> left) => PutLeft(right, left);

    /// <summary>
    /// Calls <c>PutR</c>. See <see cref="PutRight"/>
    /// </summary>
    public Result<TRight> CallPutRight(TLeft left, Option<TRight> right) => PutRight(left, right);

    /// <summary>
    /// Calls <c>CreateR</c>. See <see cref="CreateRight"/>
    /// </summary>
    public Result<TRight> CallCreateRight(TLeft left) => CreateRight(left);

    /// <summary>
    /// Calls <c>CreateL</c>. See <see cref="CreateLeft"/>
    /// </summary>
    public Result<TLeft> CallCreateLeft(TRight right) => CreateLeft(right);


    /// <summary>
    /// Inverts the lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static InvertLens<TRight, TLeft> operator !(BaseSymmetricLens<TLeft, TRight> originalLens)
        => InvertLens.Cons(originalLens);

}