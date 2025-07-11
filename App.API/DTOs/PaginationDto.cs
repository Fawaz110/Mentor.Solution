namespace App.API.DTOs
{
    public class PaginationDto<T> where T : class 
    {
        public PaginationDto(int count, int page, int limit, List<T> data)
        {
            Count = count;
            Page = page;
            Limit = limit;
            Data = data;
        }

        public int Count { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public List<T> Data { get; set; }
    }
}
