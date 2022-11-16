namespace Enigmatry.Entry.CodeGeneration.Configuration
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}
