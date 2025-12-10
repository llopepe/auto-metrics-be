namespace Core.Framework.Aplication.Common.Exceptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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