using System.Threading.Tasks;
using Enigmatry.Entry.CodeGeneration.Configuration;

namespace Enigmatry.Entry.CodeGeneration.Rendering
{
    public interface IComponentGenerator
    {
        Task GenerateAsync(string outputDir, IComponentModel component);
    }
}
