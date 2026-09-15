using System.Reflection;
using System.Runtime.InteropServices;
#if !NETFRAMEWORK
using System.Runtime.Versioning;
#endif

#if !NETFRAMEWORK
// GenerateAssemblyInfo tắt (file này tự viết tay), nên bộ SDK cũng không tự thêm attribute này
// cho TargetFramework có hậu tố "-windows" - thêm thủ công ở đây để bộ phân tích tương thích nền
// tảng ngừng cảnh báo CA1416 ở mọi chỗ gọi System.Drawing/GDI+. Attribute
// System.Runtime.Versioning.SupportedOSPlatformAttribute không tồn tại trên .NET Framework, nên
// mới có #if - net48 vốn đã chỉ chạy trên Windows nên không cần đánh dấu.
[assembly: SupportedOSPlatform("windows")]
#endif

[assembly: AssemblyTitle("ImageProcessing")]
[assembly: AssemblyDescription("Standalone scanned-document image processing library (rotate/deskew/blank-detect/barcode/TIFF/PDF/PDF-A).")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("IMIP Technology And Solution Consultancy JSC.")]
[assembly: AssemblyProduct("ImageProcessing")]
[assembly: AssemblyCopyright("Copyright (c) IMIP Technology And Solution Consultancy JSC. 2026")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: Guid("6b6d6a2e-6f6a-4b0e-9a1f-9a6b6c9b0e21")]

[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
