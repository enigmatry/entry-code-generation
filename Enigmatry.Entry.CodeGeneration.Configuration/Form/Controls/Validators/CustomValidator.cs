namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Validators;

public class CustomValidator
{
    public string Name { get; } = String.Empty;

    public CustomValidator(string name)
    {
        Name = name;
    }
}