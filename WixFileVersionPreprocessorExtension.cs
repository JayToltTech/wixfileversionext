using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Microsoft.Tools.WindowsInstallerXml;

namespace WixFileVersionExtension
{
    public class WixFileVersionPreprocessorExtension : PreprocessorExtension
    {
        private static readonly string[] prefixes = { "fileVersion" };

        public override string[] Prefixes
        {
            get { return prefixes; }
        }

        public override string EvaluateFunction(string prefix, string function, string[] args)
        {
            switch (prefix)
            {
                case "fileVersion":
                    if (args.Length == 0 || args[0].Length == 0)
                        throw new ArgumentException("File name not specified");

                    if (!File.Exists(args[0]))
                        throw new ArgumentException(string.Format("File name {0} does not exist", args[0]));

                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(args[0]);
                    PropertyInfo propertyInfo = fileVersionInfo.GetType().GetProperty(function);

                    if (propertyInfo == null)
                        throw new ArgumentException(string.Format("Unable to find property {0} in FileVersionInfo", function));

                    var value = (string)propertyInfo.GetValue(fileVersionInfo, null);

                    if (function == "FileVersion")
                    {
                        var split = value.Split('.');

                        // Check for an overly large major and split it apart if needed
                        var major = split[0];
                        if (Int32.Parse(major) > 256)
                        {
                            var m1 = major.Substring(0, major.Length - 2);
                            var m2 = major.Substring(major.Length - 2, 2);
                            return $"{m1}.{m2}.{split[1]}.{split[2]}";
                        }
                    }

                    return value;
            }

            return null;
        }
    }
}
