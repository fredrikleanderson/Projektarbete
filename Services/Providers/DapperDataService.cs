using Dapper;
using Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Models;
using Models.Entities;
using Services.Interfaces;
using Services.Providers;

namespace DapperConnection
{
    public class DapperDataService: DataService, IDataService
    {
        public DapperDataService(
            DataContext context, 
            IOptions<DatabaseSettings> dbOptions, 
            IMappingService dataHandler, 
            IQueryStringService queryStringService) : base(context, dbOptions, dataHandler, queryStringService) 
        { 
        
        }

        public async Task PostUsersAsync(IEnumerable<CreateUserModel> models)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryStringService.InsertUsers(models.ToArray()));
            }
        }

        public async Task PostPostsAsync(IEnumerable<CreatePostModel> models)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryStringService.InsertPosts(models.ToArray()));
            }
        }

        public async Task PostLikesAsync(IEnumerable<CreateLikeModel> models)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryStringService.InsertLikes(models.ToArray()));
            }
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var users = await connection.QueryAsync<User>(_queryStringService.SelectAllUsers());
                return users.Select(user => _mappingService.MapUser(user));
            }
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersByIdAsync(IEnumerable<UserModel> models)
        {
            List<UserModel> result = new();

            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var model in models)
                {
                    var user = await connection.QuerySingleAsync<User>(_queryStringService.SelectUserById(model));
                    if (user != null)
                        result.Add(_mappingService.MapUser(user));
                }
            }

            return result;
        }

        public async Task<IEnumerable<PostModel>> GetAllPostsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var posts = await connection.QueryAsync<Post, User, Post>(_queryStringService.SelectAllPosts(),
                    (post, user) =>
                    {
                        post.User = user;
                        return post;
                    });
                return posts.Select(post => _mappingService.MapPost(post, post.User));
            }
        }

        public async Task<IEnumerable<LikesPerPostModel>> GetMostLikedPosts(int numberOfPosts)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var likes = await connection.QueryAsync<Like, Post, User, Like>(_queryStringService.SelectMostLikedPost(),
                    (like, post, user) =>
                    {
                        like.Post = post;
                        like.Post.User = user;
                        return like;
                    });

                var groups = likes
                    .GroupBy(key => key.PostId)
                    .OrderByDescending(group => group.Count())
                    .Take(numberOfPosts);

                return groups.Select(group => _mappingService.MapPostWithLikes(group));
            }
        }

        public async Task PutAllUsersAsync(IEnumerable<UpdateUserModel> models)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var model in models)
                {
                    await connection.QueryAsync(_queryStringService.UpdateSingleUser(model));
                }
            }
        }

        public async Task ClearAllTablesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryStringService.ClearDatabase());
            }
        }
    }
}