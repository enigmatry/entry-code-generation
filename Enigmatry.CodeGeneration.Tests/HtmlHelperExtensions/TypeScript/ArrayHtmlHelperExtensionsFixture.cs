using System.IO;
using System.Text.Encodings.Web;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.HtmlHelperExtensions.TypeScript
{
    public class ArrayHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        [TestCase(new int[] { }, ExpectedResult = "[]")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "[1, 2, 3]")]
        public string TestArrayFor(params int[] collection)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.ArrayFor(collection);
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

            return stringWriter.ToString();
        }

        [TestCase("a", "b", "c", ExpectedResult = "['a', 'b', 'c']")]
        public string TestStringArrayFor(params string[] collection)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.StringArrayFor(collection);
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

            return stringWriter.ToString();
        }
    }
}
