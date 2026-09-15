using System.Reflection;
using System.Runtime.InteropServices;
#if !NETFRAMEWORK
using System.Runtime.Versioning;
#endif

#if !NETFRAMEWORK
// GenerateAssemblyInfo is off (this file is hand-maintained), which also disables the SDK's
// usual auto-injection of this attribute for a "-windows" TargetFramework - added explicitly so
// the platform-compat analyzer stops flagging every System.Drawing/GDI+ call site with CA1416.
// System.Runtime.Versioning.SupportedOSPlatformAttribute doesn't exist on .NET Framework, hence
// the #if - net48 is inherently Windows-only anyway, nothing to annotate there.
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
