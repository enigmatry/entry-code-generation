using Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select;
using System;
using System.Collections.Generic;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.Mocks
{
    public class LookupApiControllerMock
    {
        public IEnumerable<SelectOption> GetCategoryLookups() => new List<SelectOption>();
        public IEnumerable<SelectOption> GetTypeLookups(Guid categoryId) => new List<SelectOption>();
        public IEnumerable<SelectOption> GetSubTypeLookups(Guid typeId) => new List<SelectOption>();
    }
}
