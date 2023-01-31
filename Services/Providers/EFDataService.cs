using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Services.Interfaces;

namespace Services.Providers
{
    public class EFDataService : DataService
    {
        public EFDataService(
            DataContext context,
            IOptions<DatabaseSettings> dbOptions,
            IMappingService dataHandler,
            IQueryStringService queryStringService) : base(context, dbOptions, dataHandler, queryStringService) 
        {

        }

        public override async Task PostUsersAsync(IEnumerable<CreateUserModel> models)
        {
            await _context.AddRangeAsync(models.Select(model => _mappingService.MapUser(model)));
            await _context.SaveChangesAsync();
        }

        public override async Task PostPostsAsync(IEnumerable<CreatePostModel> models)
        {
            await _context.AddRangeAsync(models.Select(model => _mappingService.MapPost(model)));
            await _context.SaveChangesAsync();
        }

        public override async Task PostLikesAsync(IEnumerable<CreateLikeModel> models)
        {
            await _context.AddRangeAsync(models.Select(model => _mappingService.MapLike(model)));
            await _context.SaveChangesAsync();
        }

        public override async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return users.Select(user => _mappingService.MapUser(user));
        }

        public override async Task<IEnumerable<UserModel>> GetAllUsersByIdAsync(IEnumerable<UserModel> models)
        {
            _context.ChangeTracker.Clear();
            List<UserModel> result = new();

            foreach (var model in models)
            {
                var user = await _context.Users.FindAsync(model.Id);
                if(user != null)
                    result.Add(_mappingService.MapUser(user));
            }

            return result;
        }

        public override async Task<IEnumerable<PostModel>> GetAllPostsAsync()
        {
            var posts = await _context.Posts
                .Include(post => post.User)
                .AsNoTracking()
                .ToListAsync();
            return posts.Select(post => _mappingService.MapPost(post, post.User));
        }

        public override async Task<IEnumerable<LikesPerPostModel>> GetMostLikedPosts(int numberOfPosts)
        {
            var likes = await _context.Likes
                .Include(like => like.Post)
                .ThenInclude(post => post.User)
                .AsNoTracking()
                .ToListAsync();

            var groups = likes
                .GroupBy(key => key.PostId)
                .OrderByDescending(group => group.Count())
                .Take(numberOfPosts);

            return groups.Select(group => _mappingService.MapPostWithLikes(group));
        }

        public override async Task PutAllUsersAsync(IEnumerable<UpdateUserModel> models)
        {
            foreach (var model in models)
            {
                var user = await _context.Users.FindAsync(model.Id);
                if (user != null)
                {
                    _mappingService.MapUser(user, model);
                    _context.Entry(user).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();
        }

        public override async Task ClearAllTablesAsync()
        {
            await _context.Likes.ExecuteDeleteAsync();
            await _context.Posts.ExecuteDeleteAsync();
            await _context.Users.ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}