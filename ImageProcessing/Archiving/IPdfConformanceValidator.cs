using System.Collections.Generic;
using System.IO;

namespace ImageProcessing.Archiving
{
    /// <summary>Kiểm tra 1 file PDF có đạt đúng mức chuẩn PDF/A hay không.</summary>
    public interface IPdfConformanceValidator
    {
        ValidationReport Validate(Stream pdf, PdfAConformance conformance);
    }

    public enum ValidationSeverity
    {
        Warning,
        Error
    }

    /// <summary>Một lỗi/cảnh báo cụ thể do validator phát hiện.</summary>
    public sealed class ValidationIssue
    {
        public ValidationIssue(string ruleId, string clause, string message, ValidationSeverity severity, int occurrences)
        {
            RuleId = ruleId ?? string.Empty;
            Clause = clause ?? string.Empty;
            Message = message ?? string.Empty;
            Severity = severity;
            Occurrences = occurrences;
        }

        public string RuleId { get; }

        /// <summary>Điều khoản ISO 19005 tương ứng với quy tắc này, nếu có.</summary>
        public string Clause { get; }

        public string Message { get; }

        public ValidationSeverity Severity { get; }

        public int Occurrences { get; }
    }

    /// <summary>Kết quả kiểm tra 1 file PDF theo mức chuẩn PDF/A.</summary>
    public sealed class ValidationReport
    {
        public ValidationReport(bool isCompliant, PdfAConformance conformance, IReadOnlyList<ValidationIssue> issues)
        {
            IsCompliant = isCompliant;
            Conformance = conformance;
            Issues = issues ?? new List<ValidationIssue>();
        }

        /// <summary>True khi tài liệu đạt đúng mức <see cref="Conformance"/> - điều kiện để pipeline CI cho qua.</summary>
        public bool IsCompliant { get; }

        public PdfAConformance Conformance { get; }

        public IReadOnlyList<ValidationIssue> Issues { get; }
    }
}
