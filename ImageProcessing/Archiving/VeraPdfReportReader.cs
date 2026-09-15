using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Đọc report XML của veraPDF (định dạng mặc định "--format xml") thành 1
    /// <see cref="ValidationReport"/>. Để riêng khỏi phần gọi tiến trình để có thể unit-test độc
    /// lập ngoại tuyến với các report mẫu đã lưu sẵn, không cần cài veraPDF thật. Việc đọc XML
    /// không phụ thuộc namespace (chỉ so khớp theo tên element/attribute cục bộ) để không bị vỡ
    /// khi veraPDF đổi phiên bản.
    /// </summary>
    public static class VeraPdfReportReader
    {
        /// <summary>
        /// Đọc report của veraPDF. Trả về báo cáo "không đạt chuẩn" kèm 1 lỗi tự sinh nếu không
        /// tìm thấy element <c>validationReport</c> nào (ví dụ veraPDF gặp lỗi xử lý/parse).
        /// </summary>
        public static ValidationReport Parse(string reportXml, PdfAConformance conformance)
        {
            if (string.IsNullOrWhiteSpace(reportXml))
                return NonCompliant(conformance, "veraPDF produced no report output.");

            XDocument doc;
            try
            {
                doc = XDocument.Parse(reportXml);
            }
            catch (Exception ex)
            {
                return NonCompliant(conformance, "veraPDF report was not valid XML: " + ex.Message);
            }

            XElement vr = Descendants(doc, "validationReport").FirstOrDefault();
            if (vr == null)
            {
                string task = Descendants(doc, "taskResult").Select(e => Attr(e, "type")).FirstOrDefault(v => !string.IsNullOrEmpty(v));
                return NonCompliant(conformance, "veraPDF returned no validationReport" + (task != null ? " (task: " + task + ")" : "") + ".");
            }

            bool isCompliant = string.Equals(Attr(vr, "isCompliant"), "true", StringComparison.OrdinalIgnoreCase);

            List<ValidationIssue> issues = new List<ValidationIssue>();
            foreach (XElement rule in Descendants(vr, "rule"))
            {
                if (!string.Equals(Attr(rule, "status"), "failed", StringComparison.OrdinalIgnoreCase))
                    continue;

                string spec = Attr(rule, "specification");
                string clause = Attr(rule, "clause");
                string test = Attr(rule, "testNumber");
                int failedChecks = ParseInt(Attr(rule, "failedChecks"), 1);
                string description = Descendants(rule, "description").Select(e => e.Value).FirstOrDefault();

                issues.Add(new ValidationIssue(
                    BuildRuleId(spec, clause, test), clause ?? string.Empty, (description ?? string.Empty).Trim(),
                    ValidationSeverity.Error, Math.Max(1, failedChecks)));
            }

            if (issues.Count > 0)
                isCompliant = false; // Đề phòng trường hợp cờ "đạt chuẩn" mâu thuẫn với danh sách rule bị lỗi.

            return new ValidationReport(isCompliant, conformance, issues);
        }

        private static ValidationReport NonCompliant(PdfAConformance conformance, string message)
        {
            List<ValidationIssue> issues = new List<ValidationIssue>
            {
                new ValidationIssue("verapdf.report", string.Empty, message, ValidationSeverity.Error, 1)
            };
            return new ValidationReport(false, conformance, issues);
        }

        private static string BuildRuleId(string specification, string clause, string testNumber)
        {
            IEnumerable<string> parts = new[] { specification, clause, testNumber }.Where(p => !string.IsNullOrEmpty(p));
            string id = string.Join(" ", parts);
            return string.IsNullOrEmpty(id) ? "verapdf.rule" : id;
        }

        private static IEnumerable<XElement> Descendants(XContainer root, string localName)
        {
            return root.Descendants().Where(e => e.Name.LocalName == localName);
        }

        private static string Attr(XElement e, string localName)
        {
            XAttribute a = e.Attributes().FirstOrDefault(x => x.Name.LocalName == localName);
            return a != null ? a.Value : null;
        }

        private static int ParseInt(string s, int fallback)
        {
            int v;
            return int.TryParse(s, out v) ? v : fallback;
        }
    }
}
