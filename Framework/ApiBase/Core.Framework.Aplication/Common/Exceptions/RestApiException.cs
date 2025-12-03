namespace Core.Framework.Aplication.Common.Exceptions
{
    public class RestApiException : Exception
    {
        public RestApiException()
        {
        }

        public RestApiException(string message) : base(message)
        {
        }

        public RestApiException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}