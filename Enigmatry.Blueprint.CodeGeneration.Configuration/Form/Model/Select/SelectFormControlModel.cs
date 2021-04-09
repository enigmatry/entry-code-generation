using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControlModel : FormControlModel
    {
        public LookupMethodBase LookupMedhod { get; set; } = null!;

        public bool UsesCustomLookupMethod => LookupMedhod is CustomLookupMethod;
    }
}
