using Enigmatry.Entry.CodeGeneration.Configuration;

namespace Enigmatry.Entry.CodeGeneration.Rendering;

public interface IComponentGenerator
{
    Task GenerateAsync(CodeGeneratorOptions options, string outputDir, IComponentModel component);
}
