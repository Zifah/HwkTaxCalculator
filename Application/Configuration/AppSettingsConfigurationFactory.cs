using Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Application.Configuration
{
    public class AppSettingsConfigurationFactory : IConfigurationFactory
    {
        private readonly IConfiguration _configuration;

        public AppSettingsConfigurationFactory(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public IConfigProvider GetConfigProvider(string sectionName)
        {
            return new AppSettingsConfigProvider(_configuration, sectionName);
        }
    }
}
