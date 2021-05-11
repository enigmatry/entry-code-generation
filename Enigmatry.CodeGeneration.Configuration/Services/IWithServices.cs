using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.Services
{
    public interface IWithServices
    {
        IEnumerable<IServiceModel> Services { get; }
    }
}
