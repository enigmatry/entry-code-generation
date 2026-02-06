using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular;

public abstract class AngularCodeGeneratorFixtureBase : CodeGenerationFixtureBase
{
    protected abstract string ExpectedResultsLocation { get; }

    protected void AssertGeneratedFileNames(IList<string> generatedFilePaths)
    {
        var expectedFileNames = Directory.GetFiles($"{ExpectedResultsLocation}")
            .Select(filePath => filePath.Split(Path.DirectorySeparatorChar).Last().Replace(".txt", ""));

        foreach (var expectedFileName in expectedFileNames)
        {
            generatedFilePaths
                .Any(path => path.Contains(expectedFileName))
                .ShouldBeTrue();
        }
    }

    protected void AssertGeneratedFileContent(IEnumerable<(string Path, string Content)> generatedFiles)
    {
        foreach (var (path, generatedContent) in generatedFiles)
        {
            var fileName = $"{path.Split(Path.DirectorySeparatorChar).Last()}.txt";
            var expectedContent = File.ReadAllText($"{ExpectedResultsLocation}/{fileName}");

            Console.WriteLine($"Validation against {fileName} file!");

            Uglify(generatedContent)
                .ShouldBe(Uglify(expectedContent), fileName);
        }
    }

    // Dirty Hack
    protected static string Uglify(string input) => input.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
}

