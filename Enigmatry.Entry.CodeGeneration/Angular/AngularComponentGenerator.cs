using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.List;
using Enigmatry.Entry.CodeGeneration.Rendering;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Enigmatry.Entry.CodeGeneration.Angular;

[UsedImplicitly]
public class AngularComponentGenerator : IComponentGenerator
{
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IEnumerable<TemplateInfo> _componentTemplates;
    private readonly ILogger<AngularComponentGenerator> _logger;

    public AngularComponentGenerator(
        ITemplateRenderer templateRenderer,
        AngularSettings angularSettings,
        ILogger<AngularComponentGenerator> logger)
    {
        _templateRenderer = templateRenderer;
        _logger = logger;
        _componentTemplates = angularSettings.Component.Templates;
    }

    public async Task GenerateAsync(string outputDir, IComponentModel component)
    {
        var componentDirectory = Path.Combine(outputDir, component.AngularComponentDirectory());

        foreach (TemplateInfo templateInfo in _componentTemplates)
        {
            var templatePath = templateInfo.TemplatePath.FormatWith(GetTemplateNameFor(component));

            var fileName = templateInfo.FileNamingPattern.FormatWith(component.ComponentInfo.Name.Kebaberize());
            var filePath = Path.Combine(componentDirectory, fileName);

            await _templateRenderer.RenderAndSaveToFileAsync(templatePath, component, filePath);

            _logger.LogInformation($"   {fileName} generated");
        }
    }

    private static string GetTemplateNameFor(IComponentModel component)
    {
        return component switch
        {
            ListComponentModel _ => "List",
            FormComponentModel _ => "Form",
            _ => throw new ArgumentOutOfRangeException(nameof(component))
        };
    }
}