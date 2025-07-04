using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecifications<TEntity> where TEntity : BaseEntity
    {
        public List<Expression<Func<TEntity, bool>>> Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
