using System.Text.RegularExpressions;

namespace Zaher.Ui.Helpers
{
    public static class StripHtml
    {
        private const string HTML_TAG_PATTERN = "<.*?>";

        public static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }
    }
}
