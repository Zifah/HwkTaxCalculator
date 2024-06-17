namespace Core.Configuration
{
    public interface IConfigProvider
    {
        T? GetValue<T>(string key);

        /// <summary>
        /// Return the entire configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>();
    }
}
