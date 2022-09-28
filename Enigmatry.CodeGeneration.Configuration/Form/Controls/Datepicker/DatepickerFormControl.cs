namespace Enigmatry.CodeGeneration.Configuration.Form.Controls.Datepicker
{
    public class DatepickerFormControl : FormControl
    {
        public DatepickerFormControl()
        {
            ValueUpdateTrigger = Controls.ValueUpdateTrigger.OnBlur;
        }

        public override string FormlyType => "datepicker";
    }
}
