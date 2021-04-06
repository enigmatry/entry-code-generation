namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select
{
    public struct SelectOption
    {
        public object Value { get; private set; }
        public string DisplayName { get; private set; }

        public SelectOption(object value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }
    }
}
