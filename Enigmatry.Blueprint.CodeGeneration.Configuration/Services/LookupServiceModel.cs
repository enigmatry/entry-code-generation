﻿using System;
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

        public IEnumerable<FixedValuesLookupMethod> FixedValuesLookupMethods =>
            LookupMethods.Where(method => method is FixedValuesLookupMethod).Select(method => (FixedValuesLookupMethod)method);

        public IEnumerable<CustomLookupMethod> CustomLookupMethods =>
            LookupMethods.Where(method => method is CustomLookupMethod).Select(method => (CustomLookupMethod)method);
    }
}
