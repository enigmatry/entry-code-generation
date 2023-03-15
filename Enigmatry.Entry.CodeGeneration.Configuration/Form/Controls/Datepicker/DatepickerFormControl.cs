namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DatepickerFormControl : FormControl
{
    public DatepickerFormControl()
    {
        ValueUpdateTrigger = Controls.ValueUpdateTrigger.OnBlur;
    }

    public override string FormlyType => "datepicker";
}