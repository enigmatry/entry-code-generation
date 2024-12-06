namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

public class BooleanPropertyFormatter : BasePropertyFormatter
{
    public override IList<Type> SupportedInputTypes() => new List<Type> {typeof(bool)};

    public override string JsFormatterName => "boolean";

    public override string ToJsObject() => $"{{ name: \'{JsFormatterName}\' }}";
}