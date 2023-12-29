namespace Bifrons.Base;
/// <summary>
/// Either monad
/// </summary>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public readonly struct Either<TLeft, TRight>
{
    private readonly Option<TLeft> _left;
    private readonly Option<TRight> _right;

    /// <summary>
    /// Is this a left value?
    /// </summary>
    public bool IsLeft => _left.IsSome;

    /// <summary>
    /// Is this a right value?
    /// </summary>
    public bool IsRight => _right.IsSome;

    /// <summary>
    /// Left value
    /// </summary>
    public TLeft Left => _left.Value;

    /// <summary>
    /// Right value
    /// </summary>
    public TRight Right => _right.Value;

    /// <summary>
    /// Constructor
    /// </summary>
    internal Either(Option<TLeft> left, Option<TRight> right)
    {
        _left = left;
        _right = right;
    }
}

/// <summary>
/// Either monad
/// </summary>
public static class Either
{
    /// <summary>
    /// Create a left value
    /// </summary>
    public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        => new Either<TLeft, TRight>(value, Option.None<TRight>());

    /// <summary>
    /// Create a right value
    /// </summary>
    public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        => new Either<TLeft, TRight>(Option.None<TLeft>(), value);


    public static Either<RLeft, RRight> Map<TLeft, TRight, RLeft, RRight>(
        this Either<TLeft, TRight> either,
        Func<TLeft, RLeft> lmap,
        Func<TRight, RRight> rmap)
        => either.IsLeft
            ? Left<RLeft, RRight>(lmap(either.Left))
            : Right<RLeft, RRight>(rmap(either.Right));

    public static Either<RLeft, TRight> MapLeft<TLeft, TRight, RLeft>(
        this Either<TLeft, TRight> either,
        Func<TLeft, RLeft> lmap)
        => either.IsLeft
            ? Left<RLeft, TRight>(lmap(either.Left))
            : Right<RLeft, TRight>(either.Right);

    public static Either<TLeft, RRight> MapRight<TLeft, TRight, RRight>(
        this Either<TLeft, TRight> either,
        Func<TRight, RRight> rmap)
        => either.IsLeft
            ? Left<TLeft, RRight>(either.Left)
            : Right<TLeft, RRight>(rmap(either.Right));

    public static Either<RLeft, RRight> Bind<TLeft, TRight, RLeft, RRight>(
        this Either<TLeft, TRight> either,
        Func<TLeft, Either<RLeft, RRight>> lbind,
        Func<TRight, Either<RLeft, RRight>> rbind)
        => either.IsLeft
            ? lbind(either.Left)
            : rbind(either.Right);

    public static Either<RLeft, TRight> BindLeft<TLeft, TRight, RLeft>(
        this Either<TLeft, TRight> either,
        Func<TLeft, Either<RLeft, TRight>> lbind)
        => either.IsLeft
            ? lbind(either.Left)
            : Right<RLeft, TRight>(either.Right);

    public static Either<TLeft, RRight> BindRight<TLeft, TRight, RRight>(
        this Either<TLeft, TRight> either,
        Func<TRight, Either<TLeft, RRight>> rbind)
        => either.IsLeft
            ? Left<TLeft, RRight>(either.Left)
            : rbind(either.Right);
}

