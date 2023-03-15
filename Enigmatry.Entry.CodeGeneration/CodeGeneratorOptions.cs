using System;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration;

public class CodeGeneratorOptions
{
    public CodeGeneratorOptions(string outputDirectory, Assembly sourceAssembly, bool enableI18N)
    {
        OutputDirectory = outputDirectory;
        SourceAssembly = sourceAssembly;
        EnableI18N = enableI18N;
    }

    public string OutputDirectory { get; set; }
    public Assembly SourceAssembly { get; set; }
    public Framework Framework { get; set; } = Framework.Angular;
    public string Component{ get; set; } = String.Empty;
    public string Feature { get; set; } = String.Empty;
    public string GeneratedComponentPrefix { get; set; } = "app-g";
    public string ApiClientTsImportPath { get; set; } = "src/app/api/api-reference";
    public string ValidatorsPath { get; set; } = "src/app/shared/validators/custom-validators";
    public bool EnableI18N { get; set; } = false;
}