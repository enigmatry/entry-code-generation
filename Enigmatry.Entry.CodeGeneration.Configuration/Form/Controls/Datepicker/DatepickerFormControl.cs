namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DatepickerFormControl : FormControl
{
    public override string ControlType => ControlTypes.DatePicker;
    public DateTimeOffset? DefaultValue { get; set; }

    public DatepickerFormControl()
    {
        ValueUpdateTrigger = Controls.ValueUpdateTrigger.OnBlur;
    }
}
