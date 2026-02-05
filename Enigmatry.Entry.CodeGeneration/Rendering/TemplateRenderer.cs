using Enigmatry.Entry.TemplatingEngine;
using JetBrains.Annotations;

namespace Enigmatry.Entry.CodeGeneration.Rendering;

[UsedImplicitly]
public class TemplateRenderer(ITemplatingEngine templatingEngine, ITemplateWriter templateWriter) : ITemplateRenderer
{
    public async Task<string> RenderAsync<T>(string templatePath, T model) => await templatingEngine.RenderFromFileAsync(templatePath, model);

    public async Task<string> RenderAsync<T>(string templatePath, T model, IDictionary<string, object> viewBag) => await templatingEngine.RenderFromFileAsync(templatePath, model, viewBag);

    public async Task RenderAndSaveToFileAsync<T>(string templatePath, T model, string filePath)
    {
        var contents = await RenderAsync(templatePath, model);
        await templateWriter.WriteToFileAsync(filePath, contents);
    }

    public async Task RenderAndSaveToFileAsync<T>(string templatePath, T model, IDictionary<string, object> viewBag, string filePath)
    {
        var contents = await RenderAsync(templatePath, model, viewBag);
        await templateWriter.WriteToFileAsync(filePath, contents);
    }
}
