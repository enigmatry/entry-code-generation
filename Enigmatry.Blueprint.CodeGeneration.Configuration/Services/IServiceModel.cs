using System.Collections.Generic;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Services
{
    public interface IServiceModel
    {
        public string Name { get; set; }
        public IEnumerable<IServiceMethod> Methods { get; set; }
    }
}
