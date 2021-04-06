using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enigmatry.Blueprint.CodeGeneration.Configuration;
using Enigmatry.Blueprint.CodeGeneration.Rendering;
using Microsoft.Extensions.Logging;

namespace Enigmatry.Blueprint.CodeGeneration
{
    public class CodeGenerator
    {
        private readonly IModuleGenerator _moduleGenerator;
        private readonly CodeGenerationOptions _options;
        private readonly ILogger<CodeGenerator> _logger;
        private IList<IFeatureModule> _featureModules;

        public CodeGenerator(
            IModuleGenerator moduleGenerator,
            CodeGenerationOptions options,
            ILogger<CodeGenerator> logger)
        {
            _moduleGenerator = moduleGenerator;
            _options = options;
            _logger = logger;
            _featureModules = new List<IFeatureModule>();
        }

        public async Task Generate()
        {
            _logger.LogInformation($"Generating {_options.Framework} modules ...");

            BuildAllDefinitions();
            await GenerateFiles();
        }

        private void BuildAllDefinitions()
        {
            var sourceAssembly = _options.GenerateFromAssembly;

            _featureModules = _options.HasComponentName
                ? ConfigurationScanner
                    .FindComponentsInAssemblyByName(sourceAssembly, _options.ComponentName)
                    .GroupByFeature()
                    .ToList()
                : ConfigurationScanner
                    .FindComponentsInAssembly(sourceAssembly)
                    .GroupByFeature()
                    .ToList();
        }

        private async Task GenerateFiles()
        {
            foreach (var feature in _featureModules)
            {
                await _moduleGenerator.GenerateAsync(_options.OutputDir, feature);
            }
        }
    }
}
