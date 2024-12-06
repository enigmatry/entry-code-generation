namespace Enigmatry.Entry.CodeGeneration.Configuration;

public static class TypeExtensions
{
    /// <summary>
    /// Gets type name including declaring (parent) type names
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetDeclaringName(this Type type)
    {
        var parentType = type;
        var parentTypes = new List<Type>();
        while (true)
        {
            if (parentType.DeclaringType != null)
            {
                parentTypes.Insert(0, parentType.DeclaringType);
                parentType = parentType.DeclaringType;
                continue;
            }
            break;
        }

        if (parentTypes.Count > 0)
        {
            var typeNames = parentTypes.Select(parent => parent.Name);
            return $"{String.Join(String.Empty, typeNames)}{type.Name}";
        }

        return type.Name;
    }
}