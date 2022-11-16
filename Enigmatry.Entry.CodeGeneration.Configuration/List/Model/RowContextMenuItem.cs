using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model
{
    public class RowContextMenuItem
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string? Icon { get; set; }
        public string? TranslationId { get; set; }
    }
}
