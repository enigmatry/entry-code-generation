using Enigmatry.Blueprint.CodeGeneration.Configuration.Services;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select
{
    public class SelectFormControlModel : FormControlModel
    {
        public SelectFormControlType SelectType { get; set; }
        public LookupMethodBase LookupMedhod { get; set; } = null!;
    }
}
