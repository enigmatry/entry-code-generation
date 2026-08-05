namespace Enigmatry.Entry.CodeGeneration.Configuration;

public interface IComponentModel
{
    ComponentInfo ComponentInfo { get; }
    bool? WithSignals { get; }
}