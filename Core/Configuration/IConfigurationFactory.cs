namespace Core.Configuration
{
    public interface IConfigurationFactory
    {
        IConfigProvider GetConfigProvider(string sectionName);
    }
}
