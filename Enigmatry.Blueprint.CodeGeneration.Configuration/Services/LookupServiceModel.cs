using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Services
{
    public class LookupServiceModel : IServiceModel
    {
        public string Name { get; set; } = String.Empty;
        public IEnumerable<IServiceMethod> Methods { get; set; } = new List<LookupMethodBase>();


        public IEnumerable<LookupMethodBase> LookupMethods =>
            Methods.Where(method => method is LookupMethodBase).Select(method => (LookupMethodBase)method);

        public IEnumerable<AsyncLookupMethod> AsyncLookupMethods =>
            LookupMethods.Where(method => method is AsyncLookupMethod).Select(method => (AsyncLookupMethod)method);

        public IEnumerable<FixedLookupMethod> FixedLookupMethods =>
            LookupMethods.Where(method => method is FixedLookupMethod).Select(method => (FixedLookupMethod)method);
    }
}
