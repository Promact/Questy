using System.Text.RegularExpressions;

namespace Promact.Trappist.Utility.ExtensionMethods
{
    public static class StringExtensions    
    {
        /// <summary>
        /// This method will remove all leading, trailing 
        /// and extra white spaces within the string. 
        /// </summary>
        /// <param name="value">string to trim spaces</param>
        /// <returns>string after removing extra white spaces.</returns>
        public static string AllTrim(this string value)
        {
            return Regex.Replace(value.Trim(), "\\s+", string.Empty);
        }
    }
}
