using System.Threading.Tasks;
using Enigmatry.Blueprint.CodeGeneration.Configuration;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
{
    public interface IComponentGenerator
    {
        Task GenerateAsync(string outputDir, IComponentModel component);
    }
}
