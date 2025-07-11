using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<TEntity> : ISpecifications<TEntity> where TEntity : BaseEntity
    {
        #region Constructors
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<TEntity, bool>> _criteria)
        {
            Criteria.Add(_criteria);
        } 
        #endregion

        #region Properties
        public List<Expression<Func<TEntity, bool>>> Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
        List<Expression<Func<TEntity, bool>>> ISpecifications<TEntity>.Criteria { get; set; }
        #endregion

        #region Methods
        public void ApplyPagination(int _take, int _skip)
        {
            IsPaginationEnabled = true;
            Skip = _skip;
            Take = _take;
        }

        public void AddOrderBy(Expression<Func<TEntity, object>> _orderBy)
        {
            OrderBy = _orderBy;
        }
        public void AddOrderByDesc(Expression<Func<TEntity, object>> _orderByDesc)
        {
            OrderByDesc = _orderByDesc;
        }
        #endregion
    }
}
