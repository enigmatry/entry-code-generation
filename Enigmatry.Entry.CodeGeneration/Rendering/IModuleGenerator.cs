using System.Threading.Tasks;
using Enigmatry.Entry.CodeGeneration.Configuration;

namespace Enigmatry.Entry.CodeGeneration.Rendering;

public interface IModuleGenerator
{
    Task GenerateAsync(string outputDir, IFeatureModule module);
}