using Core;
using Core.Entities;
using Core.Service.Contract;
using Core.Specifications;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Post> AddAsync(Post post)
        {
            await _unitOfWork.Repository<Post>().AddAsync(post);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return post;
        }

        public async Task<int> DeleteAsync(string postId)
        {
            var item = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);

            if (item is null)
                return 0;

            _unitOfWork.Repository<Post>().Delete(item);

            var result = await _unitOfWork.CompleteAsync();

            if (result == 0)
                return 0;
            else if (result < 0)
                return -1;
            else
                return result;           
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(PostPaginationQueryParams queryParams, bool tracking = false)
        {
            var spec = new PostSpecification(queryParams);

            return tracking ?
                         await _unitOfWork.Repository<Post>().GetAllAsync(spec)
                       : await _unitOfWork.Repository<Post>().GetAllWithNoTrackingAsync(spec);
        }

        public async Task<bool> UpdateAsync(Post post)
        {
            var spec = new PostSpecification(post.Id);

            var item = await _unitOfWork.Repository<Post>().GetWithSpecAsync(spec);

            if (item is null) return false;

            if (post.Caption != item.Caption)
                item.Caption = post.Caption;

            _unitOfWork.Repository<Post>().Update(item);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return false;

            return true;
        }
    }
}
