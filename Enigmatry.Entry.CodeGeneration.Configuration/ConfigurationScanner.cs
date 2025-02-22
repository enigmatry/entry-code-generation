﻿using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration;

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

    public IEnumerable<IComponentModel> GetComponents()
    {
        return FindComponentConfigurations().Select(BuildComponentConfiguration);
    }

    private IEnumerable<(Type type, Type matchingInterface)> FindComponentConfigurations()
    {
        var openGenericType = typeof(IComponentConfiguration<>);

        return from type in _types
            where !type.IsAbstract && !type.ContainsGenericParameters
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