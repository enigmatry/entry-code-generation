using Enigmatry.Blueprint.CodeGeneration.Configuration.Formatters;
using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.List.Model
{
    public class ColumnPropertyModel : IPropertyModel 
    {
        public string Property { get; set; } = String.Empty;
        public string DisplayName { get; set; } = String.Empty;
        public bool IsVisible { get; set; }
        public bool IsSortable { get; set; }
        public IPropertyFromatter Formatter { get; set; } = new NoFormattingPropertyFormatter();

        public string? CustomComponentName { get; set; }
        public bool UseCustomComponent => CustomComponentName.HasContent();
    }
}
