namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

public abstract class BasePropertyFormatter : IPropertyFormatter
{
    public abstract string JsFormatterName { get; }

    public abstract IList<Type> SupportedInputTypes();

    public abstract string ToJsObject();

    public virtual bool ValidateInputType(Type inputType) => SupportedInputTypes().Any(x => x.Name == inputType.Name)
        ? true
        : throw new ArgumentOutOfRangeException($"Property of type {inputType} is not compatible with property formatter.");
}