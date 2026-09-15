using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Validates PDFs by shelling out to the veraPDF (PDF Association reference validator)
    /// command line - invoked as an external process, never linked, so its GPLv3/MPLv2 licence
    /// stays a build/CI concern and never ships inside a closed-source consumer.
    ///
    /// Invocation: <c>verapdf --flavour &lt;2b&gt; --format xml &lt;file.pdf&gt;</c>. The exit code is NOT
    /// used for compliance (veraPDF returns 1 for a non-compliant-but-processed file); compliance
    /// comes from the report XML, parsed by <see cref="VeraPdfReportReader"/>.
    /// </summary>
    public sealed class VeraPdfConformanceValidator : IPdfConformanceValidator
    {
        private readonly string _executablePath;
        private readonly int _timeoutMs;

        /// <param name="executablePath">Path to verapdf/verapdf.bat. When null, discovery tries VERAPDF_PATH, then PATH, then common install dirs.</param>
        /// <param name="timeoutMs">Kill the process after this long (default 120s).</param>
        public VeraPdfConformanceValidator(string executablePath = null, int timeoutMs = 120000)
        {
            _executablePath = string.IsNullOrEmpty(executablePath) ? VeraPdfLocator.Find() : executablePath;
            _timeoutMs = timeoutMs;
        }

        public string ExecutablePath { get { return _executablePath; } }

        public bool IsAvailable { get { return !string.IsNullOrEmpty(_executablePath) && File.Exists(_executablePath); } }

        public ValidationReport Validate(Stream pdf, PdfAConformance conformance)
        {
            if (pdf == null) throw new ArgumentNullException(nameof(pdf));
            if (!IsAvailable)
                throw new VeraPdfNotAvailableException("veraPDF executable not found. Set VERAPDF_PATH or pass the path explicitly.");

            string tempPdf = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pdf");
            try
            {
                using (FileStream fs = new FileStream(tempPdf, FileMode.CreateNew, FileAccess.Write))
                    pdf.CopyTo(fs);

                string reportXml = RunVeraPdf(conformance.Flavour(), tempPdf);
                return VeraPdfReportReader.Parse(reportXml, conformance);
            }
            finally
            {
                try { if (File.Exists(tempPdf)) File.Delete(tempPdf); } catch { /* best effort */ }
            }
        }

        /// <summary>Convenience overload for a file on disk.</summary>
        public ValidationReport Validate(string pdfPath, PdfAConformance conformance)
        {
            using (FileStream fs = File.OpenRead(pdfPath))
                return Validate(fs, conformance);
        }

        private string RunVeraPdf(string flavour, string pdfPath)
        {
            // veraPDF on Windows is a .bat launcher, which cannot be started directly with
            // UseShellExecute=false, so route it through cmd.exe.
            bool isBatch = _executablePath.EndsWith(".bat", StringComparison.OrdinalIgnoreCase)
                        || _executablePath.EndsWith(".cmd", StringComparison.OrdinalIgnoreCase);

            string innerArgs = "--flavour " + flavour + " --format xml " + Quote(pdfPath);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            if (isBatch)
            {
                psi.FileName = Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe";
                // /s + wrapping quotes: cmd strips the outermost pair, preserving inner quoting.
                psi.Arguments = "/s /c \"" + Quote(_executablePath) + " " + innerArgs + "\"";
            }
            else
            {
                psi.FileName = _executablePath;
                psi.Arguments = innerArgs;
            }

            StringBuilder stdout = new StringBuilder();
            StringBuilder stderr = new StringBuilder();
            using (Process proc = new Process { StartInfo = psi })
            {
                proc.OutputDataReceived += (s, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
                proc.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                if (!proc.WaitForExit(_timeoutMs))
                {
                    try { proc.Kill(); } catch { /* ignore */ }
                    throw new VeraPdfException("veraPDF timed out after " + _timeoutMs + " ms.");
                }
                proc.WaitForExit();

                string report = stdout.ToString();
                if (string.IsNullOrWhiteSpace(report))
                    throw new VeraPdfException("veraPDF produced no report (exit " + proc.ExitCode + "). stderr: " + stderr);
                return report;
            }
        }

        private static string Quote(string s) { return "\"" + s + "\""; }
    }

    /// <summary>Locates a veraPDF executable across configuration/PATH/install dirs.</summary>
    public static class VeraPdfLocator
    {
        private static readonly string[] CandidateNames = { "verapdf.bat", "verapdf.cmd", "verapdf.exe", "verapdf" };

        public static string Find()
        {
            string env = Environment.GetEnvironmentVariable("VERAPDF_PATH");
            if (!string.IsNullOrEmpty(env))
            {
                if (File.Exists(env)) return env;
                if (Directory.Exists(env))
                {
                    string inDir = FirstExisting(env);
                    if (inDir != null) return inDir;
                }
            }

            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string dir in path.Split(Path.PathSeparator))
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                string hit = FirstExisting(dir.Trim());
                if (hit != null) return hit;
            }

            foreach (string dir in CommonInstallDirs())
            {
                string hit = FirstExisting(dir);
                if (hit != null) return hit;
            }

            return null;
        }

        private static string FirstExisting(string dir)
        {
            foreach (string name in CandidateNames)
            {
                try
                {
                    string full = Path.Combine(dir, name);
                    if (File.Exists(full)) return full;
                }
                catch { /* invalid path element */ }
            }
            return null;
        }

        private static System.Collections.Generic.IEnumerable<string> CommonInstallDirs()
        {
            foreach (string v in new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            })
            {
                if (!string.IsNullOrEmpty(v))
                    yield return Path.Combine(v, "veraPDF");
            }
            yield return @"C:\verapdf";
        }
    }

    public class VeraPdfException : Exception
    {
        public VeraPdfException(string message) : base(message) { }
    }

    public sealed class VeraPdfNotAvailableException : VeraPdfException
    {
        public VeraPdfNotAvailableException(string message) : base(message) { }
    }
}
