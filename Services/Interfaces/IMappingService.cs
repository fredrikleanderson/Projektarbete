using Models;
using Models.Entities;
using System.Reflection.Metadata;

namespace Services.Interfaces
{
    public interface IMappingService
    {
        User MapUser(CreateUserModel model);
        UserModel MapUser(User user);
        void MapUser(User user, UpdateUserModel model);
        Post MapPost(CreatePostModel model);
        PostModel MapPost(Post post, User user);
        Like MapLike(CreateLikeModel model);
        LikesPerPostModel MapLikesPerPost(int likes, Post post, User user);
    }
}