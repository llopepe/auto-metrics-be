namespace Core.Framework.Aplication.Common.Exceptions
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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