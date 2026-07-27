namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public static class ControlTypes
{
    public const string Input = "input";
    public const string CheckBox = "checkbox";
    public const string DatePicker = "datepicker";
    public const string DateTimePicker = "datetimepicker";
    public const string Radio = "radio";
    public const string Select = "select";
    public const string Autocomplete = "autocomplete";
    public const string MultiCheckBox = "multicheckbox";
    public const string TextArea = "textarea";
    public const string Button = "button";
}

[Obsolete("Use ControlTypes instead.")]
public static class FormlyTypes
{
    public const string Input = ControlTypes.Input;
    public const string CheckBox = ControlTypes.CheckBox;
    public const string DatePicker = ControlTypes.DatePicker;
    public const string DateTimePicker = ControlTypes.DateTimePicker;
    public const string Radio = ControlTypes.Radio;
    public const string Select = ControlTypes.Select;
    public const string Autocomplete = ControlTypes.Autocomplete;
    public const string MultiCheckBox = ControlTypes.MultiCheckBox;
    public const string TextArea = ControlTypes.TextArea;
    public const string Button = ControlTypes.Button;
}
