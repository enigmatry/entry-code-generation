using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enigmatry.Entry.CodeGeneration;

public class CodeGenerator
{
    private readonly IModuleGenerator _moduleGenerator;
    private readonly CodeGeneratorOptions _options;
    private readonly ILogger<CodeGenerator> _logger;
    private IList<IComponentModel> _components = [];

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

    private void LogStart()
    {
        _logger.LogInformation("Generating {Framework} components", _options.Framework);
        _logger.LogInformation("Output dir {Directory}", _options.OutputDirectory);
        _logger.LogInformation("I18N: {I18nEnabled}", _options.EnableI18N ? "enabled" : "disabled");
        _logger.LogInformation("Standalone components: {WithStandaloneComponents}", _options.WithStandaloneComponents ? "supported" : "unsupported");
        _logger.LogInformation("Generated components prefix: {Prefix}", _options.GeneratedComponentPrefix);
        if (_options.ValidatorsPath.HasContent())
        {
            _logger.LogInformation("Validators dir {Directory}", _options.ValidatorsPath);
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
        _logger.LogInformation("");
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

        foreach (IFeatureModule feature in _components.GroupByFeature())
        {
            await _moduleGenerator.GenerateAsync(_options.OutputDirectory, feature);
        }
    }

    private void LogComponentsCount()
    {
        _logger.LogInformation("Found {Number} component(s)", _components.Count);
    }

    private void LogEnd()
    {
        _logger.LogInformation("End" + Environment.NewLine);
    }
}
