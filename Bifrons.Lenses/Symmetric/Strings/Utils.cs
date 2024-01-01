using System.Text.RegularExpressions;

namespace Bifrons.Lenses;

public static class Utils
{
    public static string GetNonMatchingValueToEnd(this Regex regex, string input)
    {
        var match = regex.Match(input);
        if (!match.Success)
        {
            return input;
        }

        var nonMatchingValue = input.Substring(match.Index + match.Length);
        return nonMatchingValue;
    }

    public static string GetNonMatchingValueFromStart(this Regex regex, string input)
    {
        var match = regex.Match(input);
        if (!match.Success)
        {
            return input;
        }

        var nonMatchingValue = input.Substring(0, match.Index);
        return nonMatchingValue;
    }
}
