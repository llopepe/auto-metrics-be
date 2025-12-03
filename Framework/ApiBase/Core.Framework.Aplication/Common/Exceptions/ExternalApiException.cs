namespace Core.Framework.Aplication.Common.Exceptions
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException()
        {
        }

        public ExternalApiException(string message) : base(message)
        {
        }

        public ExternalApiException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}