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
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }

            // Load HTML content
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Bỏ hẳn nội dung <script>/<style> - InnerText vẫn giữ text bên trong 2 thẻ này
            var nonTextNodes = doc.DocumentNode.SelectNodes("//script|//style");
            if (nonTextNodes != null)
            {
                foreach (var node in nonTextNodes)
                {
                    node.Remove();
                }
            }

            // Extract plain text, decode HTML entities, gộp khoảng trắng/xuống dòng thừa
            var text = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
            return System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
        }
    }
}
