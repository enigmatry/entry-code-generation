using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Rendering;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Enigmatry.Entry.CodeGeneration.Angular;

[UsedImplicitly]
public class AngularModuleGenerator(IComponentGenerator componentGenerator, ITemplateRenderer templateRenderer,
    AngularSettings angularSettings, ILogger<AngularModuleGenerator> logger) : IModuleGenerator
{
    public async Task GenerateAsync(CodeGeneratorOptions options, IFeatureModule module)
    {
        var directory = Path.Combine(options.OutputDirectory, module.Name.Kebaberize(), "generated");

        logger.LogInformation("Generating {ModuleName} feature module", module.Name);

        foreach (var component in module.Components)
        {
            logger.LogInformation(" Generating {ComponentName} component", component.ComponentInfo.Name);
            await componentGenerator.GenerateAsync(options, directory, component);
        }

        foreach (var templateInfo in angularSettings.GetModuleTemplates(options.WithSignals))
        {
            var fileName = templateInfo.FileNamingPattern.FormatWith(module.Name.Kebaberize());
            var filePath = Path.Combine(directory, fileName);

            await templateRenderer.RenderAndSaveToFileAsync(templateInfo.TemplatePath, module, filePath);

            logger.LogInformation("  {FileName} generated", fileName);
        }
    }
}
