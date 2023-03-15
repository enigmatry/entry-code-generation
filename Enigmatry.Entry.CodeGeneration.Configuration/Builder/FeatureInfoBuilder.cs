using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Builder;

public class FeatureInfoBuilder : IBuilder<FeatureInfo>
{
    private string _name = String.Empty;
    private readonly IList<ModuleImport> _imports = new List<ModuleImport>();

    public FeatureInfoBuilder WithName(string name)
    {
        Check.NotEmpty(name, nameof(name));

        _name = name;
        return this;
    }

    public FeatureInfoBuilder RequiresModules(string name, string path)
    {
        Check.NotEmpty(name, nameof(name));
        Check.NotEmpty(path, nameof(path));

        _imports.Add(new ModuleImport {Name = name, Path = path});
        return this;
    }

    public FeatureInfo Build()
    {
        return new FeatureInfo(_name, _imports);
    }
}