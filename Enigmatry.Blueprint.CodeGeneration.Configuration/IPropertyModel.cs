
namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public interface IPropertyModel
    {
        string Property { get; set; }
        string DisplayName { get; set; }
        bool IsVisible { get; set; }
    }
}
