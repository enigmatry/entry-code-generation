using System.Threading.Tasks;
using Enigmatry.CodeGeneration.Configuration;

namespace Enigmatry.CodeGeneration.Rendering
{
    public interface IModuleGenerator
    {
        Task GenerateAsync(string outputDir, IFeatureModule module);
    }
}
