namespace Bifrons.Lenses.Symmetric.Strings;

/// <summary>
/// Abstract class describing a simple symmetric lens between two strings.
/// <c>L : string <=> string</c>
/// </summary>
public abstract class SymmetricStringLens : BaseSymmetricLens<string, string>
{
    /// <summary>
    /// Concatenates two simple symmetric string lenses. Lens regexes have to take into account the preceding lens regexes.
    /// e.g. <c>id(\w+)</c> | <c>id((?!\w+\s)\w+)</c> is a valid lens, but <c>id(\w+)</c> | <c>id(\w+)</c> is not.
    /// </summary>
    /// <param name="lhsLens">Left-hand operand lens</param>
    /// <param name="rhsLens">Right-hand operand lens</param>
    /// <returns>SymmetricStringLens</returns>
    public static SymmetricStringLens operator |(SymmetricStringLens lhsLens, SymmetricStringLens rhsLens)
        => Combinators.Concat(lhsLens, rhsLens);

}
