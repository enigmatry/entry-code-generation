namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls
{
    public class SelectOption
    {
        public object? Value { get; }
        public I18NString DisplayName { get; }

        public SelectOption(object? value, string displayName, string translationId)
        {
            Value = value;
            DisplayName = new I18NString(translationId, displayName);
        }
    }
}
