using System.IO;
using System.Text.Encodings.Web;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Tests.TypeScript;

public class ArrayHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
{
    [TestCase(new int[] { }, ExpectedResult = "[]")]
    [TestCase(new[] {1, 2, 3}, ExpectedResult = "[1, 2, 3]")]
    public string TestArrayFor(params int[] collection)
    {
        var stringWriter = new StringWriter();

        var htmlContent = _htmlHelper.JsArray(collection);
        htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

        return stringWriter.ToString();
    }

    [TestCase("a", "b", "c", ExpectedResult = "['a', 'b', 'c']")]
    public string TestStringArrayFor(params string[] collection)
    {
        var stringWriter = new StringWriter();

        var htmlContent = _htmlHelper.JsStringArray(collection);
        htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);

        return stringWriter.ToString();
    }
}