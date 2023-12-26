namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Abstractly describes a SIMPLE symmetric lens
/// </summary>
/// <typeparam name="TLeft">Left type</typeparam>
/// <typeparam name="TRight">Right type</typeparam>
public abstract class BaseSymmetricLens<TLeft, TRight>
{
    /// <summary>
    /// putL : Y -> X? -> X
    /// </summary>
    public abstract Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft { get; }
    /// <summary>
    /// putR : X -> Y? -> Y
    /// </summary>
    public abstract Func<TLeft, Option<TRight>, Result<TRight>> PutRight { get; }
    /// <summary>
    /// createR : X? -> Y
    /// </summary>
    public abstract Func<TLeft, Result<TRight>> CreateRight { get; }
    /// <summary>
    /// createL : Y? -> X
    /// </summary>
    public abstract Func<TRight, Result<TLeft>> CreateLeft { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    protected BaseSymmetricLens() { }

    /// <summary>
    /// Call PutL
    /// </summary>
    public Result<TLeft> CallPutLeft(TRight right, Option<TLeft> left) => PutLeft(right, left);

    /// <summary>
    /// Call PutR
    /// </summary>
    public Result<TRight> CallPutRight(TLeft left, Option<TRight> right) => PutRight(left, right);

    /// <summary>
    /// Call CreateR
    /// </summary>
    public Result<TRight> CallCreateRight(TLeft left) => CreateRight(left);

    /// <summary>
    /// Call CreateL
    /// </summary>
    public Result<TLeft> CallCreateLeft(TRight right) => CreateLeft(right);



}