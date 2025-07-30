using Core.Entities;
using Core.Specifications;

namespace Core.Service.Contract
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPostsAsync(PostPaginationQueryParams queryParams);
        Task<Post> AddAsync(Post post);
        Task<bool> UpdateAsync(Post post);
        Task<int> DeleteAsync(string postId);
    }
}
