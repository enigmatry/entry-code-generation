using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enigmatry.CodeGeneration.Configuration;
using Enigmatry.CodeGeneration.Rendering;
using Microsoft.Extensions.Logging;

namespace Enigmatry.CodeGeneration
{
    public class CodeGenerator
    {
        private readonly IModuleGenerator _moduleGenerator;
        private readonly IComponentGenerator _componentGenerator;
        private readonly CodeGeneratorOptions _options;
        private readonly ILogger<CodeGenerator> _logger;
        private IList<IComponentModel> _components = new List<IComponentModel>();

        public CodeGenerator(
            IModuleGenerator moduleGenerator,
            IComponentGenerator componentGenerator,
            CodeGeneratorOptions options,
            ILogger<CodeGenerator> logger)
        {
            _moduleGenerator = moduleGenerator;
            _componentGenerator = componentGenerator;
            _options = options;
            _logger = logger;
        }

        public async Task Generate()
        {
            LogStart();

            BuildDefinitions();

            if (_components.Count == 0)
            {
                LogNoComponentsFound();
                return;
            }

            LogComponentsCount();

            await GenerateFiles();

            LogEnd();
        }

        private void BuildDefinitions()
        {
            _components = _options.ComponentName.HasContent()
                ? ConfigurationScanner.FindComponentsInAssembly(_options.SourceAssembly, _options.ComponentName).ToList()
                : ConfigurationScanner.FindComponentsInAssembly(_options.SourceAssembly).ToList();
        }

        private async Task GenerateFiles()
        {
            if (_options.ComponentName.HasContent())
            {
                await _componentGenerator.GenerateAsync(_options.OutputDirectory, _components.First());
                return;
            }

            foreach (var feature in _components.GroupByFeature())
            {
                await _moduleGenerator.GenerateAsync(_options.OutputDirectory, feature);
            }
        }

        private void LogStart()
        {
            _logger.LogInformation("Generating {Framework} components", _options.Framework);
            _logger.LogInformation("Output dir {Directory}", _options.OutputDirectory);
            _logger.LogInformation("Searching component definitions in assembly {Assembly}", _options.SourceAssembly.GetName().Name + ".dll");
        }

        private void LogNoComponentsFound() => _logger.LogInformation("No components found. Ending.");
        private void LogComponentsCount() => _logger.LogInformation("Found {Number} component(s)", _components.Count);
        private void LogEnd() => _logger.LogInformation("End");
    }
}
