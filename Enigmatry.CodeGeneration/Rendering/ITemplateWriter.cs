using System.Threading.Tasks;

namespace Enigmatry.CodeGeneration.Rendering
{
    public interface ITemplateWriter
    {
        Task WriteToFileAsync(string path, string contents);
    }
}
