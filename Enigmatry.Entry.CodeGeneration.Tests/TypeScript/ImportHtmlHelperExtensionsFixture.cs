using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.TypeScript
{
    public class ImportHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        [TestCase("", "", ExpectedResult = "")]
        [TestCase("", "src", ExpectedResult = "")]
        [TestCase("ProjectsClient", "src/app/api", ExpectedResult = "import { ProjectsClient } from 'src/app/api';")]
        public string TestImportStatement(string imports, string from)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.ImportStatement(imports, from);
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

            return stringWriter.ToString();
        }

        [TestCase(new string[] { }, "", ExpectedResult = "")]
        [TestCase(new[] {"GetProjects", "ProjectsClient"}, "src/app/api", ExpectedResult = "import { GetProjects, ProjectsClient } from 'src/app/api';")]
        public string TestImportStatementWithMultipleImports(IEnumerable<string> imports, string from)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.ImportStatement(imports, from);
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

            return stringWriter.ToString();
        }
    }
}
