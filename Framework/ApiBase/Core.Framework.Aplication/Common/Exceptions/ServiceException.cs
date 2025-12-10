namespace Core.Framework.Aplication.Common.Exceptions
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ServiceException : Exception
    {
        public ServiceException()
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}