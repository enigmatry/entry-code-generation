using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public interface IControlBuilder
    {
        PropertyInfo? PropertyInfo { get; }

        bool Has(PropertyInfo propertyInfo);

        bool Has(string propertyName);

        FormControl Build(ComponentInfo componentInfo);
    }
}
