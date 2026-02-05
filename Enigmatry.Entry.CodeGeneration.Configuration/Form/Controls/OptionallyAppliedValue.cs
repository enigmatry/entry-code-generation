namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class OptionallyAppliedValue<T>(T value, ApplyWhen when)
{
    public T Value { get; } = value;
    public ApplyWhen When { get; } = when;

    public string ApplyOptionally() => When switch
    {
        ApplyWhen.FormIsReadonly => $"${{this.applyOptionally('{this}', this.isReadonly)}}",
        ApplyWhen.FormIsNotReadonly => $"${{this.applyOptionally('{this}', !this.isReadonly)}}",
        // ReSharper disable once RedundantSwitchExpressionArms
        ApplyWhen.Always => $"{this}",
        _ => $"{this}"
    };

    public override string ToString() => Value?.ToString() ?? String.Empty;
}

public enum ApplyWhen
{
    Always,
    FormIsReadonly,
    FormIsNotReadonly
}
