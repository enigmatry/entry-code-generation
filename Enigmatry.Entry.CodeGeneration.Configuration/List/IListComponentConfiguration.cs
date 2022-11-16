namespace Enigmatry.Entry.CodeGeneration.Configuration.List
{
    public interface IListComponentConfiguration<T> : IComponentConfiguration<ListComponentBuilder<T>> where T : class
    {
    }
}
