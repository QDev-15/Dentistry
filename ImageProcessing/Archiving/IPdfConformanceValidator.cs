using System.Collections.Generic;
using System.IO;

namespace ImageProcessing.Archiving
{
    /// <summary>Validates a PDF against a PDF/A conformance level.</summary>
    public interface IPdfConformanceValidator
    {
        ValidationReport Validate(Stream pdf, PdfAConformance conformance);
    }

    public enum ValidationSeverity
    {
        Warning,
        Error
    }

    /// <summary>A single failed check reported by the validator.</summary>
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

        /// <summary>ISO 19005 clause the rule maps to, when available.</summary>
        public string Clause { get; }

        public string Message { get; }

        public ValidationSeverity Severity { get; }

        public int Occurrences { get; }
    }

    /// <summary>Result of validating a PDF against a PDF/A conformance level.</summary>
    public sealed class ValidationReport
    {
        public ValidationReport(bool isCompliant, PdfAConformance conformance, IReadOnlyList<ValidationIssue> issues)
        {
            IsCompliant = isCompliant;
            Conformance = conformance;
            Issues = issues ?? new List<ValidationIssue>();
        }

        /// <summary>True when the document conforms to <see cref="Conformance"/> - the CI gate condition.</summary>
        public bool IsCompliant { get; }

        public PdfAConformance Conformance { get; }

        public IReadOnlyList<ValidationIssue> Issues { get; }
    }
}
