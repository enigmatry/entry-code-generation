using System;
using Enigmatry.CodeGeneration.Configuration.Formatters;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class ColumnDefinition
    {
        public string Property { get; set; } = String.Empty;
        public string HeaderName { get; set; } = String.Empty;
        public IPropertyFormatter Formatter { get; set; } = new NoFormattingPropertyFormatter();
        public bool IsVisible { get; set; }
        public bool IsSortable { get; set; }
        public string? TranslationId { get; set; }
        public string? CustomCellComponent { get; set; }
        public bool HasCustomCellComponent => CustomCellComponent.HasContent();
    }
}
