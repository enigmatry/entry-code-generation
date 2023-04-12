using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class DatepickerFormControl : FormControl
{
    public override string FormlyType => FormlyTypes.DatePicker;
    public DateTimeOffset? DefaultValue { get; set; }

    public DatepickerFormControl()
    {
        ValueUpdateTrigger = Controls.ValueUpdateTrigger.OnBlur;
    }
}
