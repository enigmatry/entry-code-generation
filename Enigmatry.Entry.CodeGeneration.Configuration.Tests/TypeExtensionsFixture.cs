using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Configuration.Tests;

[Category("unit")]
public class TypeExtensionsFixture
{
    [Test]
    public void Test_GetDeclaringName_ShouldIncludeParentTypeNames()
    {
        var type = typeof(GetProjects.Response.Item);
        var declaringName = type.GetDeclaringName();
        declaringName.Should().Be("GetProjectsResponseItem");
    }

    [Test]
    public void Test_GetDeclaringName_ShouldIncludeTypeNameWhenThereAreNoParentTypes()
    {
        var type = typeof(Model);
        var declaringName = type.GetDeclaringName();
        declaringName.Should().Be("Model");
    }
}

internal static class GetProjects
{
    public class Response
    {
        public class Item
        {
        }
    }
}

internal class Model
{
}
