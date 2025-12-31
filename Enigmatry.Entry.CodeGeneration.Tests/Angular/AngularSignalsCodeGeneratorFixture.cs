using Enigmatry.Entry.CodeGeneration.Rendering;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular;

public class AngularSignalsCodeGeneratorFixture : AngularCodeGeneratorFixtureBase
{
    protected override string ExpectedResultsLocation => "Angular/FilesToBeGenerated/Signals";

    [SetUp]
    public new void Setup()
    {
        _enableI18N = true;
        _withSignals = true;
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
}
