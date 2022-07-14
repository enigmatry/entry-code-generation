
namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class DatepickerFormControl: FormControl
    {
        public DatepickerFormControl()
        {
            ValueUpdateTrigger = Validators.ValueUpdateTrigger.OnBlur;
        }

        public override string FormlyType => "datepicker";
    }
}
