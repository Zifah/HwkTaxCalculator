namespace Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Request was not in a valid format.")
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
