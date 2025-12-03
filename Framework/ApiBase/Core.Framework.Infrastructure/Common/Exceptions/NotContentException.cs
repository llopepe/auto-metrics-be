namespace Core.Framework.Infrastructure.Common.Exceptions
{
    public class NotContentException : Exception
    {
        public NotContentException()
        {
        }

        public NotContentException(string message) : base(message)
        {
        }

        public NotContentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
