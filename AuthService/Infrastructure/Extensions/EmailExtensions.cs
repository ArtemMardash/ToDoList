using System.Globalization;
using System.Text.RegularExpressions;

namespace AuthService.Infrastructure.Extensions;

public static class EmailExtensions
{
    /// <summary>
    /// check if email valid and suit for regular expression
    /// </summary>
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        try
        {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            string DomainMapper(Match match)
            {
                var idn = new IdnMapping();

                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (ArgumentException e)
        {
            return false;
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}