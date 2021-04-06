using System.Collections.Generic;

namespace Enigmatry.Blueprint.CodeGeneration.Angular
{
    public class AngularSettings
    {
        public ComponentSettings Component { get; set; } = new ComponentSettings();
        public ModuleSettings Module { get; set; } = new ModuleSettings();
        public ServiceSettings Service { get; set; } = new ServiceSettings();
    }

    public class ComponentSettings
    {
        public IList<TemplateInfo> Templates { get; } = new List<TemplateInfo>
        {
            new TemplateInfo("~/Templates/Angular.{0}.Component.cshtml", "{0}.component.ts"),
            new TemplateInfo("~/Templates/Angular.{0}.View.cshtml", "{0}.component.html"),
            new TemplateInfo("~/Templates/Angular.{0}.Style.cshtml", "{0}.component.scss")
        };
    }

    public class ModuleSettings
    {
        public IList<TemplateInfo> Templates { get; } = new List<TemplateInfo>
        {
            new TemplateInfo("~/Templates/Angular.Module.cshtml", "{0}.module.ts"),
            new TemplateInfo("~/Templates/Angular.Module.Routing.cshtml", "{0}-routing.module.ts")
        };
    }

    public class ServiceSettings
    {
        public TemplateInfo LookupServiceTemplate { get; } = new TemplateInfo("~/Templates/Angular.Lookup.Service.cshtml", "{0}-lookup.service.ts");

    }
}
