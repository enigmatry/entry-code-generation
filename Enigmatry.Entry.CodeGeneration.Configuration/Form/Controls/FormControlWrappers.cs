using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class FormControlWrappers
{
    public static readonly FormControlWrappers Default = new FormControlWrappers(Enumerable.Empty<string>());

    // TODO: move to configuration
    private IEnumerable<string> DefaultWrappers { get; } = new[] { "form-field" };
    public IEnumerable<string> CustomWrappers { get; }
    public IEnumerable<string> AllWrappers => DefaultWrappers.Where(wrapperName => !CustomWrappers.Contains(wrapperName)).Concat(CustomWrappers);

    public FormControlWrappers(IEnumerable<string> customWrappers)
    {
        CustomWrappers = customWrappers.ToList();
    }
}