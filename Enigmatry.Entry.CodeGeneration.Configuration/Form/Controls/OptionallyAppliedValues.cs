namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class OptionallyAppliedValues<T>
{
    private readonly IList<OptionallyAppliedValue<T>> _values = new List<OptionallyAppliedValue<T>>();

    public IEnumerable<OptionallyAppliedValue<T>> Values => _values.AsEnumerable();

    public void Add(OptionallyAppliedValue<T> value)
    {
        _values.Add(value);
    }
}
