using System;
using System.Linq.Expressions;
using Enigmatry.Blueprint.CodeGeneration.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.Configuration
{
    [Category("unit")]
    public class ExpressionExtensionsFixture
    {
        [Test]
        public void TestGetPropertyInfoFromPropertyExpression()
        {
            Expression<Func<Model1, string>> propertyExpression = model => model.Title;

            var propertyInfo = propertyExpression.GetPropertyAccess();

            propertyInfo.Should().NotBeNull();
            propertyInfo.Name.Should().Be(nameof(Model1.Title));
        }

        internal class Model1
        {
            public string Title { get; set; } = String.Empty;
        }
    }
}
