namespace Enigmatry.Blueprint.CodeGeneration.Configuration
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}
