using Enigmatry.Blueprint.CodeGeneration.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Enigmatry.Blueprint.CodeGeneration.Tests
{
    // only needed so that startup is in the same assembly as the host
    // this way we avoid Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.CompilationFailedException in the tests
    public class RazorTestStartup : RazorConsoleStartup
    {
        public RazorTestStartup(IConfiguration configuration, IHostEnvironment environment): base(configuration, environment)
        {
        }
    }
}
