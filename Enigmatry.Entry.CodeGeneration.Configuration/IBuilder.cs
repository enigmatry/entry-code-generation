namespace Enigmatry.CodeGeneration.Configuration
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}
