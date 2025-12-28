namespace Enigmatry.Entry.CodeGeneration.Angular;

public class AngularSettings
{
    private string TemplatePath { get; }
    private UiLibrary UiLibrary { get; }

    public AngularSettings(UiLibrary uiLibrary)
    {
        UiLibrary = uiLibrary;
        TemplatePath = $"~/Templates/Angular/{UiLibrary}/";
    }

    public IReadOnlyList<TemplateInfo> GetComponentTemplates(bool withSignals)
    {
        var path = DetermineAngularPath(withSignals);
        return new List<TemplateInfo> { new($"{path}.{{{0}}}.Component.cshtml", "{0}-generated.component.ts"), new($"{path}.{{{0}}}.View.cshtml", "{0}-generated.component.html") };
    }

    public IReadOnlyList<TemplateInfo> GetModuleTemplates(bool withSignals)
    {
        var path = DetermineAngularPath(withSignals);
        return new List<TemplateInfo> { new($"{path}.Module.cshtml", "{0}-generated.module.ts") };
    }

    private string DetermineAngularPath(bool withSignals) => Path.Join(TemplatePath, withSignals ? "Signals" : string.Empty, "Angular");
}
