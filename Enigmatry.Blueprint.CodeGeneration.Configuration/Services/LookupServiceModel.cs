using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Services
{
    public class LookupServiceModel : IServiceModel
    {
        public string Name { get; set; } = String.Empty;
        public IEnumerable<IServiceMethod> Methods { get; set; } = new List<LookupMethodBase>();

        public IEnumerable<LookupMethodBase> LookupMethods => Methods.OfType<LookupMethodBase>();
        public IEnumerable<AsyncLookupMethod> AsyncLookupMethods => LookupMethods.OfType<AsyncLookupMethod>();
        public IEnumerable<FixedValuesLookupMethod> FixedValuesLookupMethods => LookupMethods.OfType<FixedValuesLookupMethod>();
        public IEnumerable<CustomLookupMethod> CustomLookupMethods => LookupMethods.OfType<CustomLookupMethod>();
    }
}
