namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Describes an invert lens. L : R <=> L
/// </summary>
public class InvertLens : SymmetricStringLens
{
    private readonly SymmetricStringLens _originalLens;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    private InvertLens(SymmetricStringLens originalLens)
    {
        _originalLens = originalLens;
    }

    public override Func<string, Option<string>, Result<string>> PutLeft => _originalLens.PutRight;

    public override Func<string, Option<string>, Result<string>> PutRight => _originalLens.PutLeft;

    public override Func<string, Result<string>> CreateRight => _originalLens.CreateLeft;

    public override Func<string, Result<string>> CreateLeft => _originalLens.CreateRight;

    /// <summary>
    /// Constructs an invert lens
    /// </summary>
    /// <param name="originalLens">The original lens to invert</param>
    public static InvertLens Cons(SymmetricStringLens originalLens)
        => new(originalLens);
}
