using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDataService
    {
        Task PostUsersAsync(IEnumerable<CreateUserModel> models);
        Task PostPostsAsync(IEnumerable<CreatePostModel> models);
        Task PostLikesAsync(IEnumerable<CreateLikeModel> models);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<IEnumerable<UserModel>> GetAllUsersByIdAsync(IEnumerable<UserModel> models);
        Task<IEnumerable<PostModel>> GetAllPostsAsync();
        Task<IEnumerable<LikesModel>> GetMostLikedPosts(int numberOfPosts);
        Task PutAllUsersAsync(IEnumerable<UpdateUserModel> models);
        Task ClearAllTablesAsync();
    }
}