namespace Enigmatry.Entry.CodeGeneration.Configuration;

public interface IComponentBuilder<out T> : IBuilder<T> where T : IComponentModel
{
}