namespace Enigmatry.Entry.CodeGeneration.Configuration;

public static class ObjectExtensions
{
    private static readonly HashSet<Type> NumericTypes = new()
    {
        typeof(int),  typeof(double),  typeof(decimal),
        typeof(long), typeof(short),   typeof(sbyte),
        typeof(byte), typeof(ulong),   typeof(ushort),
        typeof(uint), typeof(float)
    };

    public static bool IsNumeric(this object o)
    {
        return NumericTypes.Contains(o.GetType());
    }
}
