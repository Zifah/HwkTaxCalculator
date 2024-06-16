using Core;
using Core.Exceptions;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Application
{
    public class AppSettingsConfigProvider : IConfigProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingsConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetValue<T>(string key)
        {
            if (typeof(T).IsPrimitive || typeof(T) == typeof(string) || typeof(T) == typeof(decimal))
            {
                var value = _configuration[key];
                if (string.IsNullOrEmpty(value))
                {
                    throw new KeyNotFoundException($"The key '{key}' was not found in the appSettings.");
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }

            var section = _configuration.GetSection(key);

            if (!section.Exists())
            {
                throw new KeyNotFoundException($"The key '{key}' was not found in the appSettings.");
            }

            var configObject = section.Get<T>();

            if (configObject == null)
            {
                throw new ConfigurationException($"Could not bind config section {key} to type: {typeof(T).Name}.");
            }

            return configObject;

        }
    }
}
