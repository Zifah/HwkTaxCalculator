using Core.Configuration;
using Core.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Application.Configuration
{
    public class AppSettingsConfigProvider : IConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string? _sectionName;

        public AppSettingsConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _sectionName = null;
        }

        public AppSettingsConfigProvider(IConfiguration configuration, string sectionName)
        {
            _configuration = configuration;
            _sectionName = sectionName;
        }

        public T? GetValue<T>(string key)
        {
            string fullKey = $"{_sectionName}:{key}".Trim(':');
            var isNullableType = Nullable.GetUnderlyingType(typeof(T)) != null;

            if (typeof(T).IsPrimitive || typeof(T) == typeof(string) || typeof(T) == typeof(decimal))
            {
                var value = _configuration[fullKey];
                if (!isNullableType && string.IsNullOrEmpty(value))
                {
                    throw new KeyNotFoundException($"The key '{fullKey}' was not found in the appSettings.");
                }
                return string.IsNullOrEmpty(value) ? default : (T)Convert.ChangeType(value, typeof(T));
            }

            var section = _configuration.GetSection(fullKey);

            if (!section.Exists())
            {
                throw new KeyNotFoundException($"The key '{fullKey}' was not found in the appSettings.");
            }

            var configObject = section.Get<T>();

            if (!isNullableType && configObject == null)
            {
                throw new ConfigurationException($"Could not bind config section {fullKey} to type: {typeof(T).Name}.");
            }

            return configObject;

        }

        public T Get<T>() 
        {
            if (string.IsNullOrWhiteSpace(_sectionName))
            {
                throw new ConfigurationException("Configuration sectionName must be set to get the entire section.");
            }

            return GetValue<T>(string.Empty)!;
        }
    }
}
