using Core.Entities;

namespace Core.Specifications
{
    public class PostSpecification : BaseSpecification<Post>
    {
        public PostSpecification(PostPaginationQueryParams queryParams)
            : base(p => 
                        (string.IsNullOrEmpty(queryParams.UserId) || queryParams.UserId == p.UserId)
            )
        {
            switch (queryParams.Order)
            {
                case "asc":
                    AddOrderBy(p => p.CreatedAt);
                    break;
                default:
                    AddOrderByDesc(p => p.CreatedAt);
                    break;
            };

            ApplyPagination((int)queryParams.Limit, (((int)queryParams.Page - 1) * (int)queryParams.Limit));

            AddIncludes();
        }

        public PostSpecification(string postId) : base (p => p.Id == postId)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.User);
            Includes.Add(p => p.Images);
        }
    }
}
