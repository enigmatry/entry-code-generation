using Enigmatry.Blueprint.CodeGeneration.Configuration.Form.Model.Select;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Services
{
    public interface IServiceMethod
    {
        string Name { get; set; } 
        IList<string> ArgumentNames { get; set; }
    }

    public abstract class LookupMethodBase : IServiceMethod
    {
        public string Name { get; set; } = String.Empty;
        public IList<string> ArgumentNames { get; set; } = new List<string>();
        public IList<LookupMethodBase> DependantMethods { get; set; } = new List<LookupMethodBase>();
        public bool TriggersOnCahngeEvent => DependantMethods.Any();
    }

    public class AsyncLookupMethod : LookupMethodBase
    {
        public string ApiClientName { get; set; } = String.Empty;

        public AsyncLookupMethod(MethodInfo methodInfo)
        {
            Name = methodInfo.Name.Camelize();
            ApiClientName = $"{methodInfo.DeclaringType?.Name.Replace("Controller", "").Pascalize()}Client";
        }
    }

    public class FixedValuesLookupMethod : LookupMethodBase
    {
        public IList<SelectOption> FixedValues { get; set; } = new List<SelectOption>();

        public FixedValuesLookupMethod(string methodName, IEnumerable<SelectOption> fixedValues)
        {
            Name = methodName.Camelize();
            FixedValues = fixedValues.ToList();
        }
    }

    public class CustomLookupMethod : LookupMethodBase
    {
        public CustomLookupMethod(string methodName)
        {
            Name = methodName;
        }
    }
}
