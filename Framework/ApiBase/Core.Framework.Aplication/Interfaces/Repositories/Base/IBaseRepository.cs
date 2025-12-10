using System.Linq.Expressions;

namespace Core.Framework.Aplication.Interfaces.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate);
        Task<IList<T>> GetAllByAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);

        // Métodos agregados
        void ClearTracking();         // Limpia todas las entidades del ChangeTracker
        void Detach(T entity);        // Desasocia una entidad específica

    }
}
