using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Rendering;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Enigmatry.CodeGeneration.Angular
{
    [UsedImplicitly]
    public class AngularModuleGenerator : IModuleGenerator
    {
        private readonly IComponentGenerator _componentGenerator;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly IEnumerable<TemplateInfo> _moduleTemplates;
        private readonly ILogger<AngularModuleGenerator> _logger;

        public AngularModuleGenerator(
            IComponentGenerator componentGenerator,
            ITemplateRenderer templateRenderer,
            AngularSettings angularSettings,
            ILogger<AngularModuleGenerator> logger)
        {
            _componentGenerator = componentGenerator;
            _templateRenderer = templateRenderer;
            _logger = logger;
            _moduleTemplates = angularSettings.Module.Templates;
        }

        public async Task GenerateAsync(string outputDir, IFeatureModule module)
        {
            var directory = Path.Combine(outputDir, module.Name.Kebaberize(), "generated");

            _logger.LogInformation("Generating {ModuleName} feature module", module.Name);

            foreach (var component in module.Components)
            {
                _logger.LogInformation(" Generating {ComponentName} component", component.ComponentInfo.Name);
                await _componentGenerator.GenerateAsync(directory, component);
            }

            foreach (var templateInfo in _moduleTemplates)
            {
                var fileName = templateInfo.FileNamingPattern.FormatWith(module.Name.Kebaberize());
                var filePath = Path.Combine(directory, fileName);

                await _templateRenderer.RenderAndSaveToFileAsync(templateInfo.TemplatePath, module, filePath);

                _logger.LogInformation($"  {fileName} generated");
            }
        }
    }
}
