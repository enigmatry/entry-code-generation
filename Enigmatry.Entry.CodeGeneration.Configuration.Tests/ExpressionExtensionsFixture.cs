using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests;

[Category("unit")]
public class ExpressionExtensionsFixture
{
    [Test]
    public void TestGetPropertyInfoFromPropertyExpression()
    {
        Expression<Func<Model1, string>> propertyExpression = model => model.Title;

        var propertyInfo = propertyExpression.GetPropertyInfo();

        propertyInfo.ShouldNotBeNull();
        propertyInfo.Name.ShouldBe(nameof(Model1.Title));
    }

    internal class Model1
    {
        public string Title { get; set; } = String.Empty;
    }
}
