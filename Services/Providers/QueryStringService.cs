using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Interfaces;

namespace Services.Providers
{
    public class QueryStringService : IQueryStringService
    {
        public string InsertUsers(CreateUserModel[] models)
        {
            StringBuilder stringBuilder = new();
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
            StringBuilder stringBuilder = new();
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
            StringBuilder stringBuilder = new();
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

        public string SelectAllUsers() => "SELECT Id, FirstName, LastName, Email FROM Users";
        public string SelectUserById(UserModel model) => $"SELECT Id, FirstName, LastName, Email FROM Users WHERE Id = {model.Id}";
        public string SelectAllPosts()
        {
            return "SELECT Posts.Id, Text, Users.Id, FirstName, LastName, Email FROM Posts " +
                "JOIN Users ON Posts.UserId = Users.Id";
        }

        public string SelectMostLikedPost()
        {
            return $"SELECT l.Id, l.PostId, l.UserId, p.Id, p.Text, p.UserId, u.Id, u.FirstName, u.LastName, u.Email FROM Likes l " +
                $"JOIN Posts p ON l.PostId = p.Id " +
                $"JOIN Users u ON p.UserId = u.Id";
        }

        public string UpdateSingleUser(UpdateUserModel model)
        {
            return $"UPDATE Users " +
                $"SET FirstName = '{model.NewFirstName}', LastName = '{model.NewLastName}', Email = '{model.NewEmail}', Password = '{model.NewPassword}' " +
                $"WHERE Id = {model.Id}";
        }

        public string ClearDatabase() => $"{DeleteLikes()}{DeletePosts()}{DeleteUsers()}";

        private string DeleteLikes() => "DELETE FROM Likes; ";

        private string DeletePosts() => "DELETE FROM Posts; ";

        private string DeleteUsers() => "DELETE FROM Users; ";
    }
}
