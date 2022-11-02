using System.Collections.Generic;
using System.Threading.Tasks;
using Enigmatry.Entry.TemplatingEngine;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Rendering
{
    [UsedImplicitly]
    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly ITemplatingEngine _templatingEngine;
        private readonly ITemplateWriter _templateWriter;

        public TemplateRenderer(ITemplatingEngine templatingEngine, ITemplateWriter templateWriter)
        {
            _templatingEngine = templatingEngine;
            _templateWriter = templateWriter;
        }

        public Task<string> RenderAsync<T>(string templatePath, T model)
        {
            return _templatingEngine.RenderFromFileAsync(templatePath, model);
        }

        public Task<string> RenderAsync<T>(string templatePath, T model, IDictionary<string, object> viewBag)
        {
            return _templatingEngine.RenderFromFileAsync(templatePath, model, viewBag);
        }

        public async Task RenderAndSaveToFileAsync<T>(string templatePath, T model, string filePath)
        {
            var contents = await RenderAsync(templatePath, model);
            await _templateWriter.WriteToFileAsync(filePath, contents);
        }

        public async Task RenderAndSaveToFileAsync<T>(string templatePath, T model, IDictionary<string, object> viewBag, string filePath)
        {
            var contents = await RenderAsync(templatePath, model, viewBag);
            await _templateWriter.WriteToFileAsync(filePath, contents);
        }
    }
}
