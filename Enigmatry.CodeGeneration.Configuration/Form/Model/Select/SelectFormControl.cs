using Enigmatry.CodeGeneration.Configuration.Services;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControl : FormControl
    {
        public LookupMethodBase LookupMethod { get; set; } = null!;

        public bool UsesCustomLookupMethod => LookupMethod is CustomLookupMethod;
    }
}
