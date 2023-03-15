namespace Enigmatry.Entry.CodeGeneration.Configuration.Form;

public interface IFormComponentConfiguration<T> : IComponentConfiguration<FormComponentBuilder<T>> where T : class
{
}