using System.Threading.Tasks;
using Enigmatry.Blueprint.BuildingBlocks.TemplatingEngine;
using JetBrains.Annotations;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
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

        public async Task RenderAndSaveToFileAsync<T>(string templatePath, T model, string filePath)
        {
            var contents = await RenderAsync(templatePath, model);
            await _templateWriter.WriteToFileAsync(filePath, contents);
        }
    }
}
