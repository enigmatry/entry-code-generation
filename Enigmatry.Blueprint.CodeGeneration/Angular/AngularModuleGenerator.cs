using Enigmatry.Blueprint.CodeGeneration.Configuration;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;
using Enigmatry.Blueprint.CodeGeneration.Rendering;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Enigmatry.Blueprint.CodeGeneration.Angular
{
    [UsedImplicitly]
    public class AngularModuleGenerator : IModuleGenerator
    {
        private readonly IComponentGenerator _componentGenerator;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly ILogger<AngularModuleGenerator> _logger;
        private readonly IEnumerable<TemplateInfo> _moduleTemplates;
        private readonly AngularSettings _angularSettings;

        public AngularModuleGenerator(
            IComponentGenerator componentGenerator,
            ITemplateRenderer templateRenderer,
            AngularSettings angularSettings,
            ILogger<AngularModuleGenerator> logger)
        {
            _componentGenerator = componentGenerator;
            _templateRenderer = templateRenderer;
            _logger = logger;
            _angularSettings = angularSettings;
            _moduleTemplates = angularSettings.Module.Templates;
        }

        public async Task GenerateAsync(string outputDir, IFeatureModule module)
        {
            var directory = Path.Combine(outputDir, module.Name.Kebaberize());

            _logger.LogInformation($"Generating {module.Name} module components");

            foreach (var component in module.Components)
            {
                _logger.LogInformation($" Found {component.ComponentInfo.Name} component");
                await _componentGenerator.GenerateAsync(directory, component);
            }

            await GenerateServiceTemplates(module.Services, directory);
            await GenerateModuleTemplates(module, directory);
        }

        private async Task GenerateModuleTemplates(IFeatureModule module, string directory)
        {
            foreach (TemplateInfo templateInfo in _moduleTemplates)
            {
                var fileName = templateInfo.FileNamingPattern.FormatWith(module.Name.Kebaberize());
                var filePath = Path.Combine(directory, fileName);

                await _templateRenderer.RenderAndSaveToFileAsync(templateInfo.TemplatePath, module, filePath);

                _logger.LogInformation($"  {fileName} generated");
            }
        }

        private async Task GenerateServiceTemplates(IEnumerable<IServiceModel> services, string directory)
        {
            var lookupServices = services.Where(service => service is LookupServiceModel);

            foreach (var lookupService in lookupServices)
            {
                var serviceDirectory = Path.Combine(directory, "services");
                var fileName = _angularSettings.Service.LookupServiceTemplate.FileNamingPattern.FormatWith(lookupService.Name.Kebaberize());
                var filePath = Path.Combine(serviceDirectory, fileName);

                await _templateRenderer.RenderAndSaveToFileAsync(_angularSettings.Service.LookupServiceTemplate.TemplatePath, lookupService, filePath);

                _logger.LogInformation($"  {fileName} generated");
            }
        }
    }
}
