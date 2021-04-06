using System.Collections.Generic;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Services
{
    public interface IWithServices
    {
        IEnumerable<IServiceModel> Services { get; }
    }
}
