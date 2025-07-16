using Enigmatry.Entry.CodeGeneration.Rendering;
using NUnit.Framework;
using Shouldly;

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
        _options.Framework.ShouldBe(Framework.Angular);

        await _codeGenerator.Generate();

        var inMemoryTemplateWriter = (InMemoryTemplateWriter)GetService<ITemplateWriter>();

        inMemoryTemplateWriter.FilesToWrite.ShouldNotBeEmpty();
        AssertGeneratedFileNames(inMemoryTemplateWriter.FilesToWrite.Select(x => x.Path).ToList());
        AssertGeneratedFileContent(inMemoryTemplateWriter.FilesToWrite);
    }


    private static void AssertGeneratedFileNames(IList<string> generatedFilePaths)
    {
        IEnumerable<string> expectedFileNames = Directory.GetFiles("Angular/FilesToBeGenerated")
            .Select(filePath => filePath.Split(Path.DirectorySeparatorChar).Last().Replace(".txt", ""));

        foreach (var expectedFileName in expectedFileNames)
        {
            generatedFilePaths
                .Any(path => path.Contains(expectedFileName))
                .ShouldBeTrue();
        }
    }

    private static void AssertGeneratedFileContent(IEnumerable<(string Path, string Content)> generatedFiles)
    {
        foreach ((string Path, string Content) generatedFile in generatedFiles)
        {
            var fileName = $"{generatedFile.Path.Split(Path.DirectorySeparatorChar).Last()}.txt";
            var expectedContent = File.ReadAllText($"Angular/FilesToBeGenerated/{fileName}");
            var generatedContent = generatedFile.Content;

            Console.WriteLine($"Validation against {fileName} file!");

            Uglify(generatedContent)
                .ShouldBe(Uglify(expectedContent), fileName);
        }
    }

    // Dirty Hack
    private static string Uglify(string input)
    {
        return input.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
    }
}
