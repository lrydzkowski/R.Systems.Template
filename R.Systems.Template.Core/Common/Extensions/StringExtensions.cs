using System.Text;

namespace R.Systems.Template.Core.Common.Extensions;

public static class StringExtensions
{
    public static string RepeatString(this string input, int times)
    {
        if (times < 0)
        {
            throw new ArgumentException($"The '{nameof(times)}' parameter must be non-negative.", nameof(times));
        }

        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        StringBuilder result = new(input.Length * times);
        for (int i = 0; i < times; i++)
        {
            result.Append(input);
        }

        return result.ToString();
    }

    public static string FirstLetterUpperCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
    }
}
