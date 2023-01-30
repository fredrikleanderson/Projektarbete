using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreatePostModel
    {
        public string Text { get; set; } = null!;
        public int UserId { get; set; }
        public static class Factory
        {
            private static Random random = new();
            private static string[] texts =
            {
                "Gud vad gott med kaffe",
                "Idag ska jag ta mig i kragen",
                "Finlandsfärja - here we go",
                "Fan vad det luktar bensin överallt - MP 2026!!",
                "På fredag blir det pizza och grogg i min ensamhet",
                "Det finns hopp för Djurgården",
                "Min bästa vän heter Gustav Berg",
                "På fredag är det byta-kön-dag-AW på jobbet - Skoj!",
                "Vet inte vad som är värst - hönan eller ägget?",
                "Älskar korv med bröd"
            };
            private static CreatePostModel CreateRandomPost(UserModel user)
            {
                return new CreatePostModel
                {
                    Text = texts[random.Next(0, texts.Length)],
                    UserId = user.Id
                };
            }

            public static IEnumerable<CreatePostModel> CreateRandomPosts(IEnumerable<UserModel> users)
            {
                return users.Select(user => CreateRandomPost(user));
            }

            public static IEnumerable<CreatePostModel> CreateRandomPosts(UserModel[] users, int numberOfPostsPerUser)
            {
                for (int i = 0; i < numberOfPostsPerUser; i++)
                {
                    for (int j = 0; j < users.Length; j++)
                    {
                        yield return CreateRandomPost(users[j]);
                    }
                }
            }
        }
    }
}
