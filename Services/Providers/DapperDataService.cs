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

        public override async Task<IEnumerable<UserPageModel>> GetUserPages(IEnumerable<int> ids)
        {
            List<UserPageModel> result = new();

            using(var connection = new SqlConnection(_connectionString))
            {
                foreach (var id in ids)
                {
                    var posts = await connection.QueryAsync<Post, User, Post>(_queryService.SelectUserWithPosts(id),
                        (post, user) =>
                        {
                            post.User = user;
                            return post;
                        });

                    result.Add(_mappingService.MapUserPage(posts));
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
                return await connection.QueryAsync<int, Post, User, LikesPerPostModel>(_queryService.SelectMostLikedPosts(numberOfPosts),
                    (likes, post, user) => _mappingService.MapLikesPerPost(likes, post, user));
            }
        }

        public override async Task PutUsersAsync(IEnumerable<UpdateUserModel> models)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var model in models)
                {
                    await connection.QueryAsync(_queryService.UpdateSingleUser(model));
                }
            }
        }

        public override async Task DeleteUserPostsAsync(IEnumerable<int> ids)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var id in ids)
                {
                    await connection.QueryAsync(_queryService.DeleteUserPosts(id));
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