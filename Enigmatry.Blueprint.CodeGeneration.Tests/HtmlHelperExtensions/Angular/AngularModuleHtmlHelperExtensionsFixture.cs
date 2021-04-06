using System;
using System.IO;
using System.Text.Encodings.Web;
using Enigmatry.Blueprint.CodeGeneration.Configuration;
using Enigmatry.Blueprint.CodeGeneration.Templates.HtmlHelperExtensions;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.HtmlHelperExtensions.Angular
{
    public class AngularModuleHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        private static readonly IComponentModel UserListComponent = new TestComponent(new ComponentInfo {Name = "UserList", FeatureName = "Users"});
        private static readonly IComponentModel UserDetailsComponent = new TestComponent(new ComponentInfo {Name = "UserDetails", FeatureName = "Users"});
        private static readonly IFeatureModule FeatureModule = new FeatureModule("Users", new[] {UserListComponent, UserDetailsComponent});

        [Test]
        public void TestImportComponentsFromModule()
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.ImportComponentsFrom(FeatureModule);
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

            var importStatements = stringWriter.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var expectedResults = new[] {"import { UserListComponent } from './user-list/user-list.component';", "import { UserDetailsComponent } from './user-details/user-details.component';"};

            importStatements.Should().BeEquivalentTo(expectedResults);
        }

        internal class TestComponent : IComponentModel
        {
            public ComponentInfo ComponentInfo { get; set; }

            public TestComponent(ComponentInfo componentInfo)
            {
                ComponentInfo = componentInfo;
            }
        }
    }
}
