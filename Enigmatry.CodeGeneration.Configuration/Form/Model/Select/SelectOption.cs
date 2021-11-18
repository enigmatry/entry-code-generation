namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectOption
    {
        public object Value { get; private set; }
        public string DisplayName { get; private set; }
        public string TranslationId { get; private set; }

        public SelectOption(object value, string displayName, string translationId)
        {
            Value = value;
            DisplayName = displayName;
            TranslationId = translationId;
        }
    }
}
