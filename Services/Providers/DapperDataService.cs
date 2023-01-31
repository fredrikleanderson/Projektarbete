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
    public class DapperDataService: DataService
    {
        public DapperDataService(
            DataContext context, 
            IOptions<DatabaseSettings> dbOptions, 
            IMappingService dataHandler, 
            IQueryService queryStringService) : base(context, dbOptions, dataHandler, queryStringService) 
        { 
        
        }

        public override async Task PostUsersAsync(IEnumerable<CreateUserModel> models)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryService.InsertUsers(models.ToArray()));
            }
        }

        public override async Task PostPostsAsync(IEnumerable<CreatePostModel> models)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryService.InsertPosts(models.ToArray()));
            }
        }

        public override async Task PostLikesAsync(IEnumerable<CreateLikeModel> models)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryService.InsertLikes(models.ToArray()));
            }
        }

        public override async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var users = await connection.QueryAsync<User>(_queryService.SelectAllUsers());
                return users.Select(user => _mappingService.MapUser(user));
            }
        }

        public override async Task<IEnumerable<UserModel>> GetAllUsersByIdAsync(IEnumerable<UserModel> models)
        {
            List<UserModel> result = new();

            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var model in models)
                {
                    var user = await connection.QuerySingleAsync<User>(_queryService.SelectUserById(model));
                    if (user != null)
                        result.Add(_mappingService.MapUser(user));
                }
            }

            return result;
        }

        public override async Task<IEnumerable<PostModel>> GetAllPostsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var posts = await connection.QueryAsync<Post, User, Post>(_queryService.SelectAllPosts(),
                    (post, user) =>
                    {
                        post.User = user;
                        return post;
                    });
                return posts.Select(post => _mappingService.MapPost(post, post.User));
            }
        }

        public override async Task<IEnumerable<LikesPerPostModel>> GetMostLikedPosts(int numberOfPosts)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var likes = await connection.QueryAsync<Like, Post, User, Like>(_queryService.SelectMostLikedPost(),
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

        public override async Task PutAllUsersAsync(IEnumerable<UpdateUserModel> models)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var model in models)
                {
                    await connection.QueryAsync(_queryService.UpdateSingleUser(model));
                }
            }
        }

        public override async Task ClearAllTablesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(_queryService.ClearDatabase());
            }
        }
    }
}