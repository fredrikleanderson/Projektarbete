using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Interfaces;

namespace Services.Providers
{
    public class QueryService : IQueryService
    {
        public string InsertUsers(CreateUserModel[] models)
        {
            StringBuilder stringBuilder = new("SET NOCOUNT ON\n");
            var chunks = models.Chunk(1000);

            foreach (var chunk in chunks)
            {
                stringBuilder.Append("INSERT INTO Users VALUES");
                for (int i = 0; i < chunk.Length - 1; i++)
                {
                    stringBuilder.Append($"('{chunk[i].FirstName}', '{chunk[i].LastName}', '{chunk[i].Email}', '{chunk[i].Password}'), ");
                }
                stringBuilder.Append($"('{chunk[^1].FirstName}', '{chunk[^1].LastName}', '{chunk[^1].Email}', '{chunk[^1].Password}'); ");
            }

            return stringBuilder.ToString();
        }

        public string InsertPosts(CreatePostModel[] models)
        {
            StringBuilder stringBuilder = new("SET NOCOUNT ON\n");
            var chunks = models.Chunk(1000);

            foreach (var chunk in chunks)
            {
                stringBuilder.Append("INSERT INTO Posts VALUES");
                for (int i = 0; i < chunk.Length - 1; i++)
                {
                    stringBuilder.Append($"('{chunk[i].Text}', '{chunk[i].UserId}'), ");
                }
                stringBuilder.Append($"('{chunk[^1].Text}', '{chunk[^1].UserId}'); ");
            }

            return stringBuilder.ToString();
        }

        public string InsertLikes(CreateLikeModel[] models)
        {
            StringBuilder stringBuilder = new("SET NOCOUNT ON\n");
            var chunks = models.Chunk(1000);

            foreach (var chunk in chunks)
            {
                stringBuilder.Append("INSERT INTO Likes VALUES");
                for (int i = 0; i < chunk.Length - 1; i++)
                {
                    stringBuilder.Append($"('{chunk[i].PostId}', '{chunk[i].UserId}'), ");
                }
                stringBuilder.Append($"('{chunk[^1].PostId}', '{chunk[^1].UserId}'); ");
            }

            return stringBuilder.ToString();
        }

        public string SelectAllUsers() => "EXEC GetAllUsers";
        public string SelectUserById(UserModel model) => $"EXEC GetUserById {model.Id}";
        public string SelectUserWithPosts(int userId) => $"EXEC GetUserPage {userId}";
        public string SelectAllPosts() => $"EXEC GetAllPosts";
        public string SelectMostLikedPosts(int quantity) => $"EXEC GetMostLikedPosts {quantity}";
        public string UpdateSingleUser(UpdateUserModel model)
        {
            return $"Exec PutUser {model.Id}, '{model.NewFirstName}', '{model.NewLastName}', '{model.NewEmail}', '{model.NewPassword}'";
        }

        public string DeleteUserPosts(int userId) => $"EXEC DeleteUserPosts {userId}";
        public string ClearDatabase() => $"EXEC ClearAllTables";
    }
}
