namespace Bifrons.Lenses.Symmetric;

/// <summary>
/// Anonymous simple symmetric lens extension methods. <c>L : X <=> Y</c>
/// </summary>
public static class SymmetricLens
{
    /// <summary>
    /// Constructs an anonymous simple symmetric lens from the given functions
    /// </summary>
    public static BaseSymmetricLens<TLeft, TRight> Cons<TLeft, TRight>(
        Func<TRight, Option<TLeft>, Result<TLeft>> putLeft,
        Func<TLeft, Option<TRight>, Result<TRight>> putRight,
        Func<TLeft, Result<TRight>> createRight,
        Func<TRight, Result<TLeft>> createLeft
    ) => new SymmetricLens<TLeft, TRight>(putLeft, putRight, createRight, createLeft);
}

/// <summary>
/// Anonymous simple symmetric lens
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public sealed class SymmetricLens<TLeft, TRight> : BaseSymmetricLens<TLeft, TRight>
{
    private Func<TRight, Option<TLeft>, Result<TLeft>> putLeft;
    private Func<TLeft, Option<TRight>, Result<TRight>> putRight;
    private Func<TLeft, Result<TRight>> createRight;
    private Func<TRight, Result<TLeft>> createLeft;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="putLeft"><c>putL : Y -> Option X -> Result X</c></param>
    /// <param name="putRight"><c>putR : X -> Option Y -> Result Y</c></param>
    /// <param name="createRight"><c>createR : Option X -> Result Y</c></param>
    /// <param name="createLeft"><c>createL : Option Y -> Result X</c></param>
    internal SymmetricLens(Func<TRight, Option<TLeft>, Result<TLeft>> putLeft, Func<TLeft, Option<TRight>, Result<TRight>> putRight, Func<TLeft, Result<TRight>> createRight, Func<TRight, Result<TLeft>> createLeft)
    {
        this.putLeft = putLeft;
        this.putRight = putRight;
        this.createRight = createRight;
        this.createLeft = createLeft;
    }

    public override Func<TRight, Option<TLeft>, Result<TLeft>> PutLeft => putLeft;

    public override Func<TLeft, Option<TRight>, Result<TRight>> PutRight => putRight;

    public override Func<TLeft, Result<TRight>> CreateRight => createRight;

    public override Func<TRight, Result<TLeft>> CreateLeft => createLeft;
}