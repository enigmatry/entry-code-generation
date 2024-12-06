namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DateTimePickerFormControl : FormControl
{
    public override string FormlyType => FormlyTypes.DateTimePicker;
    public DateTimeOffset? DefaultValue { get; set; }

    public DateTimePickerFormControl()
    {
        ValueUpdateTrigger = Controls.ValueUpdateTrigger.OnBlur;
    }
}
