using System;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls.Validators
{
    public class CustomValidator
    {
        public string Name { get; private set; } = String.Empty;
        // until completed: https://github.com/angular/angular/issues/19705
        public string Trigger { get; private set; } = String.Empty;

        public CustomValidator(string name, ValidatorTrigger trigger = ValidatorTrigger.OnBlur)
        {
            Name = name;
            Trigger = GetTrigger(trigger);
        }

        private string GetTrigger(ValidatorTrigger trigger) =>
            trigger switch
            {
                ValidatorTrigger.OnBlur => "blur",
                ValidatorTrigger.OnChange => "change",
                ValidatorTrigger.OnSubmit => "submit",
                _ => "blur",
            };
    }
}
