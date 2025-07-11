using System.ComponentModel.DataAnnotations;

namespace Core.Specifications
{
    public class UserPaginationQueryParams
    {
        public string? Term { get; set; }
        [RegularExpression("^[()(company)(student)(mentor)(admin)]$", ErrorMessage = "role must be comapny, mentor, student or admin")]
        public string? Role { get; set; }
        [RegularExpression(@"^(desc|asc)$", ErrorMessage = "order must be asc or desc")]
        public string? Order { get; set; } = "desc";
        [RegularExpression(@"^(name|username|email)$", ErrorMessage = "orderBy must be name, username or email")]
        public string? OrderBy { get; set; } = "name";

        #region Pagination Props
        private int MAXIMUM_PAGE_SIZE = 30;
        private int? limit = 10;
        public int? Limit { get => limit; set => limit = (value > MAXIMUM_PAGE_SIZE || value == null) ? 30 : value; }
        public int? Page { get; set; } = 1;
        #endregion
    }
}
