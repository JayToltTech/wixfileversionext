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

                    var value = propertyInfo.GetValue(fileVersionInfo, null);
                    return value.ToString(); ;
            }

            return null;
        }
    }
}
