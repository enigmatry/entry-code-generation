using System;
using System.IO;
using System.Text.Encodings.Web;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions;
using Enigmatry.CodeGeneration.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.HtmlHelperExtensions.Angular
{
    public class AngularModuleHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        [Test]
        public void TestImportComponentsFromModule()
        {
            var featureModule = new FeatureModule("Users", new[] { new TestComponent("UserList"), new TestComponent("UserDetails") });

            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.ImportComponentsFrom(featureModule);
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

            var imports = stringWriter.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var expected = new[] {
                "import { UserListComponent } from './user-list/user-list.component';",
                "import { UserDetailsComponent } from './user-details/user-details.component';"
            };

            imports.Should().BeEquivalentTo(expected);
        }

        private class TestComponent : IComponentModel
        {
            public ComponentInfo ComponentInfo { get; }
            public TestComponent(string name) => ComponentInfo = new ComponentInfo(name);
        }
    }
}
