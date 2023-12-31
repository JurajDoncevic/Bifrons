using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes a sequentially composed lens. 
/// R <=> L  >> L <=> P = R <=> P 
/// </summary>
public class ComposeLens : SymmetricStringLens
{
    private readonly SymmetricStringLens _lhsLens;
    private readonly SymmetricStringLens _rhsLens;

    public override Regex LeftRegex => _lhsLens.LeftRegex;
    public override Regex RightRegex => _rhsLens.RightRegex;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="lhsLens">Left-hand operand lens</param>
    /// <param name="rhsLens">Right-hand operand lens</param>
    private ComposeLens(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
    {
        _lhsLens = lhsLens;
        _rhsLens = rhsLens;
    }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (right, left) => _lhsLens.PutLeft(right, left)
                            .Bind(l => _rhsLens.PutLeft(l, left));

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (left, right) => _rhsLens.PutRight(left, right)
                            .Bind(r => _lhsLens.PutRight(r, right));

    public override Func<string, Result<string>> CreateRight =>
        left => _lhsLens.CreateRight(left)
                        .Bind(l => _rhsLens.CreateRight(l));

    public override Func<string, Result<string>> CreateLeft =>
        right => _rhsLens.CreateLeft(right)
                        .Bind(r => _lhsLens.CreateLeft(r));

    /// <summary>
    /// Constructs a sequentially composed lens.
    /// </summary>
    /// <param name="lhsLens">Left-hand operand lens</param>
    /// <param name="rhsLens">Right-hand operand lens</param>
    public static ComposeLens Cons(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => new ComposeLens(lhsLens, rhsLens);

}
