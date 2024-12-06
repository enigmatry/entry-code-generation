using JetBrains.Annotations;

namespace Enigmatry.Entry.CodeGeneration.Rendering;

[UsedImplicitly]
public class TemplateWriter : ITemplateWriter
{
    private readonly ITemplateWriterAppender _appender;

    public TemplateWriter(ITemplateWriterAppender appender)
    {
        _appender = appender;
    }

    public Task WriteToFileAsync(string path, string contents)
    {
        var directoryPath = Path.GetDirectoryName(path);
        if (!String.IsNullOrEmpty(directoryPath)) Directory.CreateDirectory(directoryPath);

        if (_appender.AppendAtStart(path)) contents = _appender.TextToAppendAtStart() + contents;
        if (_appender.AppendAtEnd(path)) contents += _appender.TextToAppendAtEnd();

        return File.WriteAllTextAsync(path, contents);
    }
}