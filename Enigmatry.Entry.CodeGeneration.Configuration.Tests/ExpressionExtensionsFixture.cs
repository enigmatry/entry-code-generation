using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Configuration.Tests;

[Category("unit")]
public class ExpressionExtensionsFixture
{
    [Test]
    public void TestGetPropertyInfoFromPropertyExpression()
    {
        Expression<Func<Model1, string>> propertyExpression = model => model.Title;

        var propertyInfo = propertyExpression.GetPropertyInfo();

        propertyInfo.Should().NotBeNull();
        propertyInfo.Name.Should().Be(nameof(Model1.Title));
    }

    internal class Model1
    {
        public string Title { get; set; } = String.Empty;
    }
}
