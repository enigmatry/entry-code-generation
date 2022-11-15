using System.Threading.Tasks;
using Enigmatry.CodeGeneration.Configuration;

namespace Enigmatry.CodeGeneration.Rendering
{
    public interface IComponentGenerator
    {
        Task GenerateAsync(string outputDir, IComponentModel component);
    }
}
