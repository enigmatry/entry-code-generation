using System.Threading.Tasks;

namespace Enigmatry.Entry.CodeGeneration.Rendering;

public interface ITemplateWriter
{
    Task WriteToFileAsync(string path, string contents);
}