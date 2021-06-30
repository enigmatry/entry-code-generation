using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Angular
{
    public class AngularSettings
    {
        public string TemplatePath { get; set; }
        public UiLibrary UiLibrary { get; set; }
        public ComponentSettings Component { get; set; } 
        public ModuleSettings Module { get; set; }
        public ServiceSettings Service { get; set; }

        public AngularSettings(UiLibrary uiLibrary)
        {
            UiLibrary = uiLibrary;
            TemplatePath = $"~/Templates/Angular/{UiLibrary}/";

            Component = new ComponentSettings(TemplatePath);
            Module = new ModuleSettings(TemplatePath);
            Service = new ServiceSettings(TemplatePath);
        }
    }

    public class ComponentSettings
    {
        public ComponentSettings([NotNull] string templatePath)
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
        public ModuleSettings([NotNull] string templatePath)
        {
            Templates = new List<TemplateInfo>
            {
                new($"{templatePath}/Angular.Module.cshtml", "{0}-generated.module.ts")
            };
        }
        public IList<TemplateInfo> Templates { get; }
    }

    public class ServiceSettings
    {
        public ServiceSettings([NotNull] string templatePath)
        {
            LookupServiceTemplate = new TemplateInfo($"{templatePath}/Angular.Lookup.Service.cshtml", "{0}-lookup.service.ts");
        }

        public TemplateInfo LookupServiceTemplate { get; }

    }
}
