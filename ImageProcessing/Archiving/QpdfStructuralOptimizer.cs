using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Optimizes PDFs by shelling out to the QPDF (Apache-2.0) command line - never linked in,
    /// so it stays a runtime/deployment concern rather than a build dependency, and one qpdf.exe
    /// serves this process regardless of its own bitness.
    ///
    /// Only structural optimization is used (object streams + Flate recompression); image
    /// XObjects (DCTDecode/CCITTFaxDecode/JPXDecode) are never re-encoded, so codecs and visual
    /// content are preserved. Object streams are permitted under PDF/A-2; re-validate downstream.
    /// </summary>
    public sealed class QpdfStructuralOptimizer : IPdfOptimizer
    {
        private readonly string _executablePath;
        private readonly int _timeoutMs;

        /// <param name="executablePath">Path to qpdf(.exe). When null, discovery tries QPDF_PATH, then an app-local "qpdf" folder, then PATH, then common install dirs.</param>
        public QpdfStructuralOptimizer(string executablePath = null, int timeoutMs = 120000)
        {
            _executablePath = string.IsNullOrEmpty(executablePath) ? QpdfLocator.Find() : executablePath;
            _timeoutMs = timeoutMs;
        }

        public string ExecutablePath { get { return _executablePath; } }

        public bool IsAvailable { get { return !string.IsNullOrEmpty(_executablePath) && File.Exists(_executablePath); } }

        public OptimizeResult Optimize(Stream input, OptimizeOptions options, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (!IsAvailable)
                throw new QpdfNotAvailableException("qpdf executable not found. Set QPDF_PATH or pass the path explicitly.");

            options = options ?? OptimizeOptions.PdfASafe();

            string inFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");
            string outFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");
            try
            {
                using (FileStream fs = new FileStream(inFile, FileMode.CreateNew, FileAccess.Write))
                    input.CopyTo(fs);
                long inputBytes = new FileInfo(inFile).Length;

                RunQpdf(BuildArguments(options, inFile, outFile));

                long outputBytes = new FileInfo(outFile).Length;
                using (FileStream fs = File.OpenRead(outFile))
                    fs.CopyTo(output);

                return new OptimizeResult(true, inputBytes, outputBytes);
            }
            finally
            {
                TryDelete(inFile);
                TryDelete(outFile);
            }
        }

        private static string BuildArguments(OptimizeOptions o, string inFile, string outFile)
        {
            List<string> args = new List<string>();
            // PDF/A clause 6.1.7.1 requires an EOL before every 'endstream'; qpdf omits it by
            // default. This flag keeps the optimized output PDF/A-conformant.
            args.Add("--newline-before-endstream");
            args.Add(o.UseObjectStreams ? "--object-streams=generate" : "--object-streams=preserve");
            if (o.RecompressStreams)
            {
                args.Add("--recompress-flate");
                args.Add("--compression-level=9");
            }
            if (o.Linearize)
                args.Add("--linearize");
            args.Add(Quote(inFile));
            args.Add(Quote(outFile));
            return string.Join(" ", args);
        }

        private void RunQpdf(string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = _executablePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            StringBuilder stderr = new StringBuilder();
            using (Process proc = new Process { StartInfo = psi })
            {
                proc.OutputDataReceived += (s, e) => { };
                proc.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                if (!proc.WaitForExit(_timeoutMs))
                {
                    try { proc.Kill(); } catch { /* ignore */ }
                    throw new QpdfException("qpdf timed out after " + _timeoutMs + " ms.");
                }
                proc.WaitForExit();

                // qpdf: 0 = success, 3 = warnings (output still produced), 2 = errors.
                if (proc.ExitCode != 0 && proc.ExitCode != 3)
                    throw new QpdfException("qpdf failed (exit " + proc.ExitCode + "): " + stderr);
            }
        }

        private static string Quote(string s) { return "\"" + s + "\""; }

        private static void TryDelete(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { /* best effort */ }
        }
    }

    /// <summary>Locates a qpdf executable across configuration/app folder/PATH/install dirs.</summary>
    public static class QpdfLocator
    {
        public static string Find()
        {
            string env = Environment.GetEnvironmentVariable("QPDF_PATH");
            if (!string.IsNullOrEmpty(env))
            {
                if (File.Exists(env)) return env;
                string inDir = FindUnder(env);
                if (inDir != null) return inDir;
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string direct = Path.Combine(baseDir, "qpdf.exe");
            if (File.Exists(direct)) return direct;
            string appQpdf = FindUnder(Path.Combine(baseDir, "qpdf"));
            if (appQpdf != null) return appQpdf;

            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string dir in path.Split(Path.PathSeparator))
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                try { string p = Path.Combine(dir.Trim(), "qpdf.exe"); if (File.Exists(p)) return p; }
                catch { /* invalid path element */ }
            }

            return FindUnder(@"C:\tools\qpdf");
        }

        private static string FindUnder(string dir)
        {
            try
            {
                if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return null;
                string direct = Path.Combine(dir, "qpdf.exe");
                if (File.Exists(direct)) return direct;
                foreach (string found in Directory.GetFiles(dir, "qpdf.exe", SearchOption.AllDirectories))
                    return found;
            }
            catch { /* ignore */ }
            return null;
        }
    }

    public class QpdfException : Exception
    {
        public QpdfException(string message) : base(message) { }
    }

    public sealed class QpdfNotAvailableException : QpdfException
    {
        public QpdfNotAvailableException(string message) : base(message) { }
    }
}
