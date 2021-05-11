using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration
{
    public class ConfigurationScanner
    {
        private readonly IEnumerable<Type> _types;

        public ConfigurationScanner(IEnumerable<Type> types)
        {
            _types = types;
        }

        public static IEnumerable<IComponentModel> FindComponentsInAssembly(Assembly assembly)
        {
            return new ConfigurationScanner(assembly.GetExportedTypes()).GetComponents();
        }

        public static IEnumerable<IComponentModel> FindComponentsInAssemblyByName(Assembly assembly, string componentName)
        {
            return FindComponentsInAssembly(assembly).Where(x => x.ComponentInfo.Name == componentName);
        }

        public static IEnumerable<IComponentModel> FindComponentsInAssemblies(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetExportedTypes().Distinct());
            return new ConfigurationScanner(types).GetComponents();
        }

        public IEnumerable<IComponentModel> GetComponents()
        {
            return FindComponentConfigurations().Select(BuildComponentConfiguration);
        }

        private IEnumerable<(Type type, Type matchingInterface)> FindComponentConfigurations()
        {
            var openGenericType = typeof(IComponentConfiguration<>);

            return from type in _types
                let interfaces = type.GetInterfaces()
                let genericInterfaces = interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                let matchingInterface = genericInterfaces.FirstOrDefault()
                where matchingInterface != null
                select (type, matchingInterface);
        }

        private static IComponentModel BuildComponentConfiguration((Type type, Type matchingInterface) configuration)
        {
            var builderType = configuration.matchingInterface.GetGenericArguments().FirstOrDefault()!;

            var builderInstance = Activator.CreateInstance(builderType) as IComponentBuilder<IComponentModel>;

            var methodInfo = configuration.type.GetMethod("Configure");

            methodInfo?.Invoke(Activator.CreateInstance(configuration.type), new object[] {builderInstance!});

            return builderInstance!.Build();
        }
    }
}
