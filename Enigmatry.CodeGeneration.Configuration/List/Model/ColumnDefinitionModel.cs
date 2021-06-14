using System;
using Enigmatry.CodeGeneration.Configuration.Formatters;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class ColumnDefinitionModel
    {
        public string Property { get; set; } = String.Empty;
        public string HeaderName { get; set; } = String.Empty;
        public bool IsVisible { get; set; }
        public bool IsSortable { get; set; }
        public IPropertyFormatter Formatter { get; set; } = new NoFormattingPropertyFormatter();
        public string? TranslationId { get; set; }
        public string? CustomCellComponent { get; set; }
        public bool UseCustomCellComponent => CustomCellComponent.HasContent();
    }
}
