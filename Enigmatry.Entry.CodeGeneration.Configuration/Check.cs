namespace Enigmatry.Entry.CodeGeneration.Configuration;

public static class Check
{
    public static T NotNull<T>(T value, string parameterName)
    {
        return value is null ? throw new ArgumentNullException(parameterName) : value;
    }

    public static string NotEmpty(string? value, string parameterName)
    {
        return (value == null || String.IsNullOrEmpty(value))
            ? throw new ArgumentNullException(parameterName)
            : value;
    }
}