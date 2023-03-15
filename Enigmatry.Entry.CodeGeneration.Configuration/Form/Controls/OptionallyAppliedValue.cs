using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls
{
    public class OptionallyAppliedValue<T>
    {
        public T Value { get; }
        public ApplyWhen When { get; }

        public OptionallyAppliedValue(T value, ApplyWhen when)
        {
            Value = value;
            When = when;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? String.Empty;
        }
    }

    public enum ApplyWhen
    {
        Always,
        FormIsReadonly,
        FormIsNotReadonly
    }
}
