using System.Threading.Tasks;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
{
    public interface ITemplateRenderer
    {
        Task<string> RenderAsync<T>(string templatePath, T model);

        Task RenderAndSaveToFileAsync<T>(string templatePath, T model, string filePath);
    }
}
