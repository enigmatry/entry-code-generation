using System.Collections.Generic;
using System.Threading.Tasks;
using Enigmatry.Entry.CodeGeneration.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Tests
{
    public class InMemoryTemplateWriter : ITemplateWriter
    {
        private readonly ITemplateWriterAppender _appender;

        public InMemoryTemplateWriter(ITemplateWriterAppender appender)
        {
            _appender = appender;
        }

        public List<(string Path, string Contents)> FilesToWrite { get; } = new();

        public Task WriteToFileAsync(string path, string contents)
        {
            if (_appender.AppendAtStart(path)) contents = _appender.TextToAppendAtStart() + contents;
            if (_appender.AppendAtEnd(path)) contents += _appender.TextToAppendAtEnd();

            FilesToWrite.Add((path, contents));

            return Task.CompletedTask;
        }
    }
}
