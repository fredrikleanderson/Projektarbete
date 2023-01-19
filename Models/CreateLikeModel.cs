using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateLikeModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public static class Factory
        {
            private static Random random = new();
            private static CreateLikeModel CreateRandomPost(PostModel post, UserModel user)
            {
                return new CreateLikeModel
                {
                    PostId = post.Id,
                    UserId = user.Id,
                };
            }

            public static IEnumerable<CreateLikeModel> CreateRandomLikes(IEnumerable<UserModel> users, PostModel[] posts, int quantity)
            {
                List<CreateLikeModel> result = new();

                foreach (var user in users)
                {
                    for (int i = 0; i < quantity; i++)
                    {
                        var randomPost = posts[random.Next(0, posts.Length)];
                        result.Add(CreateRandomPost(randomPost, user));
                    }
                }

                return result;
            }
        }
    }
}
