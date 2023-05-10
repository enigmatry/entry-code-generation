using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Angular;

public class AngularSettings
{
    private string TemplatePath { get; }
    private UiLibrary UiLibrary { get; }
    public IReadOnlyList<TemplateInfo> ComponentTemplates { get; }
    public IReadOnlyList<TemplateInfo> ModuleTemplates { get; }

    public AngularSettings(UiLibrary uiLibrary)
    {
        UiLibrary = uiLibrary;
        TemplatePath = $"~/Templates/Angular/{UiLibrary}/";

        ComponentTemplates = new List<TemplateInfo>
        {
            new($"{TemplatePath}/Angular.{{0}}.Component.cshtml", "{0}-generated.component.ts"),
            new($"{TemplatePath}/Angular.{{0}}.View.cshtml", "{0}-generated.component.html")
        };

        ModuleTemplates = new List<TemplateInfo>
        {
            new($"{TemplatePath}/Angular.Module.cshtml", "{0}-generated.module.ts")
        };
    }
}
