namespace Core.Framework.Infrastructure.Common.Exceptions
{
    public class ClientRestApiException : Exception
    {
        public ClientRestApiException()
        {
        }

        public ClientRestApiException(string message) : base(message)
        {
        }

        public ClientRestApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
