using System.Collections.Generic;
using System.Threading.Tasks;
using Enigmatry.Blueprint.CodeGeneration.Rendering;

namespace Enigmatry.Blueprint.CodeGeneration.Tests
{
    public class InMemoryTemplateWriter : ITemplateWriter
    {
        public List<(string Path, string Contents)> FilesToWrite { get; }
            = new List<(string Path, string Contents)>();

        public Task WriteToFileAsync(string path, string contents)
        {
            FilesToWrite.Add((path, contents));

            return Task.CompletedTask;
        }
    }
}
