namespace Core.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException() : base("An error occurred in the configuration.")
        {
        }

        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
