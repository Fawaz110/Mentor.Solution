using Core.Entities;
using Core.Repository.Contract;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        #region Private Fields
        private readonly MentorDbContext _context;
        #endregion

        #region Constructor
        public GenericRepository(MentorDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Inherited Methods
        public async Task AddAsync(TEntity entity)
            => await _context.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync()
            => await _context.Set<TEntity>().AsNoTracking().CountAsync();

        public async Task<int> CountAsync(ISpecifications<TEntity> specification)
            => await ApplySpecifications(specification).AsNoTracking().CountAsync();

        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specification)
            => await ApplySpecifications(specification).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllWithNoTrackingAsync(ISpecifications<TEntity> specification)
            => await ApplySpecifications(specification).AsNoTracking().ToListAsync();

        public async Task<TEntity> GetByIdAsync(string id)
            => await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<TEntity> GetByIdWithNoTrackingAsync(string id)
            => await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity> specification)
            => await ApplySpecifications(specification).FirstOrDefaultAsync();

        public async Task<TEntity> GetWithSpecWithNoTrackingAsync(ISpecifications<TEntity> specification)
            => await ApplySpecifications(specification).AsNoTracking().FirstOrDefaultAsync();

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);
        #endregion

        #region Private Methods
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity> specification)
           => SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>(), specification);
        #endregion
    }
}
