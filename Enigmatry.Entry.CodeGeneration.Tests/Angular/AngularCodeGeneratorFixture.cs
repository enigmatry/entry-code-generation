using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Enigmatry.Entry.CodeGeneration.Rendering;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular;

public class AngularCodeGeneratorFixture : CodeGenerationFixtureBase
{
    [SetUp]
    public new void Setup()
    {
        _enableI18N = true;
        base.Setup();
    }

    [Test]
    public async Task Generate()
    {
        _options.Framework.Should().Be(Framework.Angular);

        await _codeGenerator.Generate();

        var inMemoryTemplateWriter = (InMemoryTemplateWriter)GetService<ITemplateWriter>();

        inMemoryTemplateWriter.FilesToWrite.Should().NotBeEmpty();
        AssertGeneratedFileNames(inMemoryTemplateWriter.FilesToWrite.Select(x => x.Path));
        AssertGeneratedFileContent(inMemoryTemplateWriter.FilesToWrite);
    }


    private void AssertGeneratedFileNames(IEnumerable<string> generatedFilePaths)
    {
        var expectedFileNames = Directory.GetFiles("Angular/FilesToBeGenerated")
            .Select(filePath => filePath.Split("\\").Last().Replace(".txt", ""));

        foreach (var expectedFileName in expectedFileNames)
        {
            generatedFilePaths
                .Any(path => path.Contains(expectedFileName))
                .Should().BeTrue();
        }
    }

    private void AssertGeneratedFileContent(IEnumerable<(string Path, string Content)> generatedFiles)
    {
        foreach (var generatedFile in generatedFiles)
        {
            var fileName = $"{generatedFile.Path.Split("\\").Last()}.txt";
            var expectedContent = File.ReadAllText($"Angular/FilesToBeGenerated/{fileName}");
            var generatedContent = generatedFile.Content;

            System.Console.WriteLine($"Validation against {fileName} file!");

            Uglify(generatedContent)
                .Should()
                .BeEquivalentTo(Uglify(expectedContent), fileName);
        }
    }

    // Dirty Hack
    private string Uglify(string input)
        => input.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
}
