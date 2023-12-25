
using System.Text.RegularExpressions;

namespace Bifrons.Lenses.Asymmetric.Strings;

/// <summary>
/// Basic asymmetric lens that gives a predetermined constant as view and ignores it in the put. The put returns the original source
/// As presented in https://www.cs.cornell.edu/~jnfoster/papers/jnfoster-dissertation.pdf p.23
/// </summary>
public sealed class ConstantLens : BaseAsymmetricLens<string, string>
{
    private readonly string _constant;
    private readonly Regex _matchRegex; // needed for the create function create = representative(Regex)

    private ConstantLens(string constant, string matchRegex)
    {
        _constant = constant ?? string.Empty;
        _matchRegex = new Regex(matchRegex ?? string.Empty);
    }

    public override Func<string, Option<string>, Result<string>> Put =>
        (updatedView, originalSource) => originalSource ? Results.OnSuccess(originalSource.Value) : Create(updatedView);

    public override Func<string, Result<string>> Get =>
        source => Results.OnSuccess(_constant);

    public override Func<string, Result<string>> Create =>
        (view) => Results.OnSuccess(GetRepresentative(_matchRegex.ToString()));


    private string GetRepresentative(string regex)
    {
        // Use the regex pattern to generate a representative string
        var representativeString = new System.Text.StringBuilder();

        // Handle character classes
        string charClassPattern = @"\[.*?\]";
        regex = Regex.Replace(regex, charClassPattern, match =>
        {
            string charClass = match.Value;
            char representativeChar = charClass.Length > 2 ? charClass[1] : 'a';
            return representativeChar.ToString();
        });

        // Handle other special characters
        regex = Regex.Replace(regex, @"[.*+?()\\^$]", match =>
        {
            string specialChar = match.Value;
            return "\\" + specialChar;
        });

        // Generate a representative string based on the modified regex pattern
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            char randomChar = (char)random.Next('a', 'z' + 1);
            representativeString.Append(randomChar);
        }

        return representativeString.ToString();
    }
    public static ConstantLens Cons(string constant, string matchRegex)
    {
        return new ConstantLens(constant, matchRegex ?? string.Empty);
    }
}
