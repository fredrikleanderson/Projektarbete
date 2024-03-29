﻿using Models;
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
        Task<IEnumerable<UserPageModel>> GetUserPages(IEnumerable<int> ids);
        Task<IEnumerable<PostModel>> GetAllPostsAsync();
        Task<IEnumerable<LikesPerPostModel>> GetMostLikedPosts(int numberOfPosts);
        Task PutUsersAsync(IEnumerable<UpdateUserModel> models);
        Task DeleteUserPostsAsync(IEnumerable<int> ids);
        Task ClearAllTablesAsync();
    }
}