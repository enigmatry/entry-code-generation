using System.Threading.Tasks;
using Enigmatry.Blueprint.CodeGeneration.Configuration;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
{
    public interface IModuleGenerator
    {
        Task GenerateAsync(string outputDir, IFeatureModule module);
    }
}
