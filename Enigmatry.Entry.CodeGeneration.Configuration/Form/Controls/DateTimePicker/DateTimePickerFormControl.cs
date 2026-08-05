namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DateTimePickerFormControl : FormControl
{
    public override string ControlType => ControlTypes.DateTimePicker;
    public DateTimeOffset? DefaultValue { get; set; }

    public DateTimePickerFormControl()
    {
        ValueUpdateTrigger = Controls.ValueUpdateTrigger.OnBlur;
    }
}
