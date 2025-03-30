using HtmlAgilityPack;
using System.Xml;

namespace Dentistry.Web
{
    public static class Helper
    {
        public static string LimitTo(this string item, int length, string sunf)
        {
            var result = item ?? "";
            result = result.RemoveHtml();
            if (result.Length > length)
            {
                result = result.Substring(0, length);
                if (!string.IsNullOrEmpty(sunf))
                {
                    result += sunf;
                }
            }
            
            return result;
        }

        public static string RemoveHtml(this string html)
        {
            // Load HTML content
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Extract plain text and decode HTML entities
            return HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
        }
    }
}
