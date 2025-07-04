using Core.Entities;
using Core.Specifications;

namespace Core.Repository.Contract
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specification);
        Task<IEnumerable<TEntity>> GetAllWithNoTrackingAsync(ISpecifications<TEntity> specification);
        Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity> specification);
        Task<TEntity> GetWithSpecWithNoTrackingAsync(ISpecifications<TEntity> specification);
        Task<TEntity> GetByIdAsync(string id);
        Task<TEntity> GetByIdWithNoTrackingAsync(string id);
        Task<int> CountAsync();
        Task<int> CountAsync(ISpecifications<TEntity> specification);
    }
}
