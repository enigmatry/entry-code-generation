using System;
using System.Collections.Generic;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model
{
    public class ColumnDefinition
    {
        public ComponentInfo ComponentInfo { get; set; } = null!;
        public string Property { get; set; } = String.Empty;
        public string HeaderName { get; set; } = String.Empty;
        public IPropertyFormatter Formatter { get; set; } = new NoFormattingPropertyFormatter();
        public bool IsVisible { get; set; }
        public bool IsSortable { get; set; }
        public string? SortId { get; set; }
        public string TranslationId { get; set; } = String.Empty;
        public string? CustomCellComponent { get; set; }
        public string? CustomCellCssClass { get; set; }
        public IDictionary<string, string> CustomProperties { get; set; } = new Dictionary<string, string>();
        public bool HasCustomCellComponent => CustomCellComponent.HasContent();
        public bool HasCustomCellCssClass => CustomCellCssClass.HasContent();
    }
}
