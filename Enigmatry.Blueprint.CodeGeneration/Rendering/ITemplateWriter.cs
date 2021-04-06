using System.Threading.Tasks;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
{
    public interface ITemplateWriter
    {
        Task WriteToFileAsync(string path, string contents);
    }
}
