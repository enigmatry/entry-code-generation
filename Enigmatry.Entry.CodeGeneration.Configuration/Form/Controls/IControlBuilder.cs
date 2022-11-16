using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls
{
    public interface IControlBuilder
    {
        PropertyInfo? PropertyInfo { get; }

        bool Has(PropertyInfo propertyInfo);

        bool Has(string propertyName);

        FormControl Build(ComponentInfo componentInfo);
    }
}
