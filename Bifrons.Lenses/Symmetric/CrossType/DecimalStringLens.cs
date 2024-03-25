using Bifrons.Lenses.Symmetric;

namespace Bifrons.Lenses;

/// <summary>
/// Describes a decimal-string lens that tranforms decimals to strings and vice versa.
/// DecStr : decimal <=> string
/// </summary>
public sealed class DecimalStringLens : ISymmetricLens<double, string>
{
    private DecimalStringLens()
    {
    }

    public Func<string, Option<double>, Result<double>> PutLeft =>
        (string updatedView, Option<double> _) =>
            double.Parse(updatedView);

    public Func<double, Option<string>, Result<string>> PutRight =>
        (double updatedView, Option<string> _) =>
            updatedView.ToString();

    public Func<double, Result<string>> CreateRight =>
        source => source.ToString();

    public Func<string, Result<double>> CreateLeft =>
        source => double.Parse(source);

    /// <summary>
    /// Constructs a double string lens.
    /// </summary>
    public static DecimalStringLens Cons() => new();
}


/// <summary>
/// Describes a string double lens that tranforms strings to decimals and vice versa.
/// StrDec : string <=> decimal
/// </summary>
public sealed class StringDecimalLens : InvertLens<string, double>
{

    private StringDecimalLens() : base(DecimalStringLens.Cons())
    {
    }

    /// <summary>
    /// Constructs a string double lens.
    /// </summary>
    public static StringDecimalLens Cons() => new();
}