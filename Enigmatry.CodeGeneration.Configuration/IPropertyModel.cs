
namespace Enigmatry.CodeGeneration.Configuration
{
    public interface IPropertyModel
    {
        string Property { get; set; }
        string DisplayName { get; set; }
        bool IsVisible { get; set; }
    }
}
