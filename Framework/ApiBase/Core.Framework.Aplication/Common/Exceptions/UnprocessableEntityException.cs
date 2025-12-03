namespace Core.Framework.Aplication.Common.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException() { }

        public UnprocessableEntityException(string message) : base(message) { }

        public UnprocessableEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}

