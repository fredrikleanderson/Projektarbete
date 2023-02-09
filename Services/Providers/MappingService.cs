using Models;
using Models.Entities;
using Services.Interfaces;

namespace Services.Providers
{
    public class MappingService : IMappingService
    {
        public User MapUser(CreateUserModel model)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password= model.Password
            };
        }

        public UserModel MapUser(User user)
        {
            return new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        public void MapUser(User user, UpdateUserModel model)
        {
            user.FirstName = model.NewFirstName ?? user.FirstName;
            user.LastName = model.NewLastName ?? user.LastName;
            user.Email = model.NewEmail ?? user.Email;
            user.Password = model.NewPassword ?? user.Password;
        }

        public UserPageModel MapUserPage(IEnumerable<Post> posts)
        {
            var user = posts.First().User;
            return new UserPageModel
            {
                User = new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                },
                Posts = posts.Select(post => new PostModel
                {
                    Id = post.Id,
                    Text = post.Text,
                })
            };
        }

        public Post MapPost(CreatePostModel model)
        {
            return new Post
            {
                Text = model.Text,
                UserId = model.UserId,
            };
        }

        public PostModel MapPost(Post post, User user)
        {
            return new PostModel
            {
                Id = post.Id,
                Text = post.Text,
                User = new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                }
            };
        }

        public Like MapLike(CreateLikeModel model)
        {
            return new Like
            {
                PostId = model.PostId,
                UserId = model.UserId
            };
        }

        public LikesPerPostModel MapLikesPerPost(int likes, Post post, User user)
        {
            return new LikesPerPostModel
            {
                Likes= likes,
                Post = new PostModel
                {
                    Id = post.Id,
                    Text = post.Text,
                    User = new UserModel 
                    { 
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    }
                }
            };
        }
    }
}