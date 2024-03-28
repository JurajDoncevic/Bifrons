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

    public static string GetRepresentative(this Regex regex)
    {
        // Use the regex pattern to generate a representative string
        var representativeString = new System.Text.StringBuilder();
        var regexString = regex.ToString();
        // Handle character classes
        string charClassPattern = @"\[.*?\]";
        regexString = Regex.Replace(regexString, charClassPattern, match =>
        {
            string charClass = match.Value;
            char representativeChar = charClass.Length > 2 ? charClass[1] : 'a';
            return representativeChar.ToString();
        });

        // Handle other special characters
        regexString = Regex.Replace(regexString, @"[.*+?()\\^$]", match =>
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
}
