using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Enigmatry.Blueprint.CodeGeneration.Rendering
{
    [UsedImplicitly]
    public class TemplateWriter : ITemplateWriter
    {
        private readonly ILogger<TemplateWriter> _logger;

        public TemplateWriter(ILogger<TemplateWriter> logger)
        {
            _logger = logger;
        }

        public Task WriteToFileAsync(string path, string contents)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!String.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return File.WriteAllTextAsync(path, contents);
        }
    }
}
