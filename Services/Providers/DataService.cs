using Data;
using Microsoft.Extensions.Options;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers
{
    public abstract class DataService: IDataService
    {
        protected readonly DataContext _context;
        protected readonly string _connectionString = null!;
        protected readonly IMappingService _mappingService;
        protected readonly IQueryService _queryService;

        public DataService(DataContext context, IOptions<DatabaseSettings> dbOptions, IMappingService dataHandler, IQueryService queryService)
        {
            _context = context;
            _connectionString = dbOptions.Value.ConnectionString;
            _mappingService = dataHandler;
            _queryService = queryService;
        }

        public abstract Task PostUsersAsync(IEnumerable<CreateUserModel> models);
        public abstract Task PostPostsAsync(IEnumerable<CreatePostModel> models);
        public abstract Task PostLikesAsync(IEnumerable<CreateLikeModel> models);
        public abstract Task<IEnumerable<UserModel>> GetAllUsersAsync();
        public abstract Task<IEnumerable<UserModel>> GetUsersByIdAsync(IEnumerable<UserModel> models);
        public abstract Task<IEnumerable<PostModel>> GetAllPostsAsync();
        public abstract Task<IEnumerable<LikesPerPostModel>> GetMostLikedPosts(int numberOfPosts);
        public abstract Task PutUsersAsync(IEnumerable<UpdateUserModel> models);
        public abstract Task ClearAllTablesAsync();
    }
}
