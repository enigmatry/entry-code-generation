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
        private readonly CodeGeneratorOptions _options;
        private readonly ILogger<CodeGenerator> _logger;
        private IList<IComponentModel> _components = new List<IComponentModel>();

        public CodeGenerator(
            IModuleGenerator moduleGenerator,
            CodeGeneratorOptions options,
            ILogger<CodeGenerator> logger)
        {
            _moduleGenerator = moduleGenerator;
            _options = options;
            _logger = logger;
        }

        public async Task Generate()
        {
            LogStart();

            BuildDefinitions();

            await GenerateFiles();

            LogEnd();
        }

        private void BuildDefinitions()
        {
            _components = ConfigurationScanner.FindComponentsInAssembly(_options.SourceAssembly).ToList();
        }

        private async Task GenerateFiles()
        {
            if (_options.Component.HasContent())
            {
                _components = _components.Where(c => c.ComponentInfo.Name.EqualsIgnoringCase(_options.Component)).ToList();
            }

            if (_options.Feature.HasContent())
            {
                _components = _components.Where(c => c.ComponentInfo.Feature.Name.EqualsIgnoringCase(_options.Feature)).ToList();
            }

            LogComponentsCount();

            foreach (var feature in _components.GroupByFeature())
            {
                await _moduleGenerator.GenerateAsync(_options.OutputDirectory, feature);
            }
        }

        private void LogStart()
        {
            _logger.LogInformation("Generating {Framework} components", _options.Framework);
            _logger.LogInformation("Output dir {Directory}", _options.OutputDirectory);
            if (_options.EnableI18N)
            {
                _logger.LogInformation("I18N: {0}", _options.EnableI18N ? "enabled" : "disabled");
            }
            if (_options.Component.HasContent())
            {
                _logger.LogInformation("Generating component: {Component}", _options.Component);
            }
            if (_options.Feature.HasContent())
            {
                _logger.LogInformation("Generating feature: {Feature}", _options.Feature);
            }
            _logger.LogInformation("Searching component definitions in assembly {Assembly}", _options.SourceAssembly.GetName().Name + ".dll");
        }

        private void LogComponentsCount() => _logger.LogInformation("Found {Number} component(s)", _components.Count);
        private void LogEnd() => _logger.LogInformation("End");
    }
}
