using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration
{
    public class CodeGenerationOptions
    {
        public CodeGenerationOptions(string outputDir, Assembly generateFromAssembly)
        {
            OutputDir = outputDir;
            GenerateFromAssembly = generateFromAssembly;
        }

        public string OutputDir { get; set; }
        public string ComponentName { get; set; } = String.Empty;
        public Assembly GenerateFromAssembly { get; set; }
        public Framework Framework { get; set; } = Framework.Angular;


        public bool HasComponentName => !String.IsNullOrWhiteSpace(ComponentName);
    }
}
