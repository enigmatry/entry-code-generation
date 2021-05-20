using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration
{
    public class CodeGeneratorOptions
    {
        public CodeGeneratorOptions(string outputDirectory, Assembly sourceAssembly)
        {
            OutputDirectory = outputDirectory;
            SourceAssembly = sourceAssembly;
        }

        public string OutputDirectory { get; set; }
        public Assembly SourceAssembly { get; set; }
        public string ComponentName { get; set; } = String.Empty;
        public Framework Framework { get; set; } = Framework.Angular;
        public string ApiClientTsImportPath { get; set; } = "src/app/api/api-reference";
    }
}
