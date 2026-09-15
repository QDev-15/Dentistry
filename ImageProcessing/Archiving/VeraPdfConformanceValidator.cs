using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Kiểm tra PDF bằng cách gọi ra dòng lệnh veraPDF (bộ công cụ kiểm định chuẩn của PDF
    /// Association) - chạy như 1 tiến trình ngoài, không liên kết tĩnh, nên giấy phép
    /// GPLv3/MPLv2 của nó chỉ là vấn đề của bước build/CI, không bao giờ bị đóng gói kèm vào ứng
    /// dụng closed-source dùng thư viện này.
    ///
    /// Cách gọi: <c>verapdf --flavour &lt;2b&gt; --format xml &lt;file.pdf&gt;</c>. Exit code KHÔNG được
    /// dùng để xác định có đạt chuẩn hay không (veraPDF trả về 1 cho cả trường hợp file xử lý
    /// được nhưng không đạt chuẩn); kết quả đạt/không đạt lấy từ nội dung report XML, được đọc
    /// bởi <see cref="VeraPdfReportReader"/>.
    /// </summary>
    public sealed class VeraPdfConformanceValidator : IPdfConformanceValidator
    {
        private readonly string _executablePath;
        private readonly int _timeoutMs;

        /// <param name="executablePath">Đường dẫn tới verapdf/verapdf.bat. Nếu để null, sẽ tự dò theo thứ tự: biến môi trường VERAPDF_PATH, rồi PATH, rồi các thư mục cài đặt thông dụng.</param>
        /// <param name="timeoutMs">Sau khoảng thời gian này thì buộc dừng tiến trình (mặc định 120 giây).</param>
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
                try { if (File.Exists(tempPdf)) File.Delete(tempPdf); } catch { /* cố gắng hết sức, bỏ qua nếu lỗi */ }
            }
        }

        /// <summary>Bản tiện dụng cho file PDF đã có sẵn trên đĩa.</summary>
        public ValidationReport Validate(string pdfPath, PdfAConformance conformance)
        {
            using (FileStream fs = File.OpenRead(pdfPath))
                return Validate(fs, conformance);
        }

        private string RunVeraPdf(string flavour, string pdfPath)
        {
            // Trên Windows, veraPDF là launcher dạng .bat, không thể chạy trực tiếp với
            // UseShellExecute=false, nên phải chạy qua cmd.exe.
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
                // /s + bọc ngoài bằng dấu ngoặc kép: cmd sẽ bóc đúng 1 lớp ngoặc ngoài cùng, giữ
                // nguyên phần ngoặc bên trong.
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
                    try { proc.Kill(); } catch { /* bỏ qua */ }
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

    /// <summary>Tìm file thực thi veraPDF qua cấu hình/PATH/thư mục cài đặt.</summary>
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
                catch { /* thành phần đường dẫn không hợp lệ */ }
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
