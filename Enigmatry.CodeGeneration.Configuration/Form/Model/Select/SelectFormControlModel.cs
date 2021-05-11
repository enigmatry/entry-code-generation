using Enigmatry.CodeGeneration.Configuration.Services;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControlModel : FormControlModel
    {
        public LookupMethodBase LookupMedhod { get; set; } = null!;

        public bool UsesCustomLookupMethod => LookupMedhod is CustomLookupMethod;
    }
}
