namespace Enigmatry.Entry.CodeGeneration.Configuration
{
    public interface IComponentConfiguration<in TBuilder> where TBuilder : IComponentBuilder<IComponentModel>
    {
        void Configure(TBuilder builder);
    }
}
