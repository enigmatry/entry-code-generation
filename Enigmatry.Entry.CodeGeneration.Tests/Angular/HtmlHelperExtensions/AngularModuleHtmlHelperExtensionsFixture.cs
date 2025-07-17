using System.Text.Encodings.Web;
using Enigmatry.Entry.CodeGeneration.Angular;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.HtmlHelperExtensions;

public class AngularModuleHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    [Test]
    public void TestImportComponentsFromModule()
    {
        var testComponent = new TestComponent("UserList");
        var featureModule = new FeatureModule("Users", [testComponent]);

        var stringWriter = new StringWriter();

        Microsoft.AspNetCore.Html.IHtmlContent htmlContent = _htmlHelper.ImportComponentsFrom(featureModule);
        htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

        var imports = stringWriter.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        imports.ShouldBe([
            $"import {{ {testComponent.AngularComponentName()} }} from './{testComponent.AngularComponentDirectory()}/{testComponent.AngularComponentFileName()}';"]);
    }

    private class TestComponent : IComponentModel
    {
        public ComponentInfo ComponentInfo { get; }

        public TestComponent(string name)
        {
            ComponentInfo = new ComponentInfo(name);
        }
    }
}
