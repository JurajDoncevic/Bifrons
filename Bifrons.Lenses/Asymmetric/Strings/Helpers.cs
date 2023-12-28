using System.Text.RegularExpressions;

namespace Bifrons.Lenses;

public static class Helpers
{
    public static string GetRepresentative(string regex)
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
}
