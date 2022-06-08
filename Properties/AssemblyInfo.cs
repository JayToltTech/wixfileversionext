using Microsoft.Tools.WindowsInstallerXml;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("WixFileVersionExtension")]
[assembly: AssemblyDefaultWixExtension(typeof(WixFileVersionExtension.WixFileVersionExtension))]