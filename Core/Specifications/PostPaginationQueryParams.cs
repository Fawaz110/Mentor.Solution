using System.ComponentModel.DataAnnotations;

namespace Core.Specifications
{
    public class PostPaginationQueryParams
    {
        public string? UserId { get; set; }
        [RegularExpression(@"^(desc|asc)$", ErrorMessage = "order must be asc or desc")]
        public string? Order { get; set; } = "desc";
        
        #region Pagination Props
        private int MAXIMUM_PAGE_SIZE = 30;
        private int? limit = 10;
        public int? Limit { get => limit; set => limit = (value > MAXIMUM_PAGE_SIZE || value == null) ? 30 : value; }
        public int? Page { get; set; } = 1;
        #endregion
    }
}
