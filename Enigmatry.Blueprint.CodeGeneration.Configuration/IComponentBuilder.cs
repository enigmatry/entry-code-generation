
namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public interface IComponentBuilder<out T> where T : IComponentModel
    {
        T Build();
    }
}
