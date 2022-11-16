using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Angular
{
    public class AngularSettings
    {
        public string TemplatePath { get; }
        public UiLibrary UiLibrary { get; }
        public ComponentSettings Component { get; } 
        public ModuleSettings Module { get; }

        public AngularSettings(UiLibrary uiLibrary)
        {
            UiLibrary = uiLibrary;
            TemplatePath = $"~/Templates/Angular/{UiLibrary}/";

            Component = new ComponentSettings(TemplatePath);
            Module = new ModuleSettings(TemplatePath);
        }
    }

    public class ComponentSettings
    {
        public ComponentSettings(string templatePath)
        {
            Templates = new List<TemplateInfo>
            {
                new($"{templatePath}/Angular.{{0}}.Component.cshtml", "{0}-generated.component.ts"),
                new($"{templatePath}/Angular.{{0}}.View.cshtml", "{0}-generated.component.html"),
                new($"{templatePath}/Angular.{{0}}.Style.cshtml", "{0}-generated.component.scss")
            };
        }
        public IList<TemplateInfo> Templates { get; }
    }

    public class ModuleSettings
    {
        public ModuleSettings(string templatePath)
        {
            Templates = new List<TemplateInfo>
            {
                new($"{templatePath}/Angular.Module.cshtml", "{0}-generated.module.ts")
            };
        }
        public IList<TemplateInfo> Templates { get; }
    }
}
