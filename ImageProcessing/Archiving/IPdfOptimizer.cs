using System.IO;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Content-preserving structural + stream optimization of an existing PDF (object/xref
    /// streams, recompression, linearization). Must not break PDF/A conformance - callers
    /// re-validate afterward; optimization runs before the final validation gate.
    /// </summary>
    public interface IPdfOptimizer
    {
        /// <param name="input">Source PDF stream.</param>
        /// <param name="options">Which optimizations to apply.</param>
        /// <param name="output">Destination stream; not closed by the optimizer.</param>
        OptimizeResult Optimize(Stream input, OptimizeOptions options, Stream output);
    }

    /// <summary>Knobs for <see cref="IPdfOptimizer.Optimize"/>.</summary>
    public sealed class OptimizeOptions
    {
        /// <summary>Rewrite using cross-reference/object streams (smaller structure).</summary>
        public bool UseObjectStreams { get; set; } = true;

        /// <summary>Recompress uncompressed/poorly-compressed streams with Flate.</summary>
        public bool RecompressStreams { get; set; } = true;

        /// <summary>Linearize ("fast web view").</summary>
        public bool Linearize { get; set; } = false;

        /// <summary>A conservative preset that is safe to run on a PDF/A document.</summary>
        public static OptimizeOptions PdfASafe()
        {
            return new OptimizeOptions { UseObjectStreams = true, RecompressStreams = true, Linearize = false };
        }
    }

    /// <summary>Outcome of <see cref="IPdfOptimizer.Optimize"/>.</summary>
    public sealed class OptimizeResult
    {
        public OptimizeResult(bool success, long inputBytes, long outputBytes, string message = null)
        {
            Success = success;
            InputBytes = inputBytes;
            OutputBytes = outputBytes;
            Message = message ?? string.Empty;
        }

        public bool Success { get; }

        public long InputBytes { get; }

        public long OutputBytes { get; }

        /// <summary>Bytes saved (may be negative if the rewrite grew the file).</summary>
        public long BytesSaved { get { return InputBytes - OutputBytes; } }

        /// <summary>Size reduction as a fraction 0..1 (0 when input size is unknown).</summary>
        public double Ratio { get { return InputBytes > 0 ? (double)BytesSaved / InputBytes : 0.0; } }

        public string Message { get; }
    }
}
