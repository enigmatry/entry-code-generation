using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Services
{
    public interface IServiceModel
    {
        public string Name { get; set; }
        public IEnumerable<IServiceMethod> Methods { get; set; }
    }
}
