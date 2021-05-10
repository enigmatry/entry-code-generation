using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
{
    public interface ITemplateRenderer
    {
        Task<string> RenderAsync<T>(string templatePath, T model);
        Task<string> RenderAsync<T>(string templatePath, T model, IDictionary<string, object> viewBag);

        Task RenderAndSaveToFileAsync<T>(string templatePath, T model, string filePath);
        Task RenderAndSaveToFileAsync<T>(string templatePath, T model, IDictionary<string, object> viewBag, string filePath);
    }
}
