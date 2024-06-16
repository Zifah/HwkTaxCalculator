namespace Core.Configuration
{
    public interface IConfigProvider
    {
        public T? GetValue<T>(string key);
    }
}
