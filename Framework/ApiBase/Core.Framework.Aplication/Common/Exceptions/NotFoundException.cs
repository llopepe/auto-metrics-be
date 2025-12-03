namespace Core.Framework.Aplication.Common.Exceptions
{
    [Serializable]
    public class NotFoundException<TEntity> : Exception where TEntity : class
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
