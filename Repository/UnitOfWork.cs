using Core;
using Core.Entities;
using Core.Repository.Contract;
using System.Collections;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MentorDbContext _context;
        private Hashtable _repositories = new Hashtable();

        public UnitOfWork(MentorDbContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories is null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
                _repositories.Add(type, new GenericRepository<TEntity>(_context));

            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
