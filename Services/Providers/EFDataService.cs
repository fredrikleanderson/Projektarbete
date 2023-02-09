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
            IQueryService queryStringService) : base(context, dbOptions, dataHandler, queryStringService) 
        {

        }

        public override async Task PostUsersAsync(IEnumerable<CreateUserModel> models)
        {
            _context.ChangeTracker.Clear();
            await _context.AddRangeAsync(models.Select(model => _mappingService.MapUser(model)));
            await _context.SaveChangesAsync();
        }

        public override async Task PostPostsAsync(IEnumerable<CreatePostModel> models)
        {
            _context.ChangeTracker.Clear();
            await _context.AddRangeAsync(models.Select(model => _mappingService.MapPost(model)));
            await _context.SaveChangesAsync();
        }

        public override async Task PostLikesAsync(IEnumerable<CreateLikeModel> models)
        {
            _context.ChangeTracker.Clear();
            await _context.AddRangeAsync(models.Select(model => _mappingService.MapLike(model)));
            await _context.SaveChangesAsync();
        }

        public override async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            _context.ChangeTracker.Clear();
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return users.Select(user => _mappingService.MapUser(user));
        }

        public override async Task<IEnumerable<UserPageModel>> GetUserPages(IEnumerable<int> ids)
        {
            _context.ChangeTracker.Clear();
            List<UserPageModel> result = new();

            foreach (var id in ids)
            {
                var posts = await _context.Posts.Include(post => post.User)
                    .AsNoTracking()
                    .Where(post => post.UserId == id)
                    .ToListAsync();

                result.Add(_mappingService.MapUserPage(posts));
            }

            return result;
        }

        public override async Task<IEnumerable<PostModel>> GetAllPostsAsync()
        {
            _context.ChangeTracker.Clear();

            var posts = await _context.Posts.Include(post => post.User)
                .AsNoTracking()
                .ToListAsync();

            return posts.Select(post => _mappingService.MapPost(post, post.User));
        }

        public override async Task<IEnumerable<LikesPerPostModel>> GetMostLikedPosts(int numberOfPosts)
        {
            _context.ChangeTracker.Clear();
            var likes = await _context.Likes
                .Include(like => like.Post)
                .ThenInclude(post => post.User)
                .AsNoTracking()
                .ToListAsync();

            return likes.GroupBy(key => key.PostId)
                .OrderByDescending(group => group.Count())
                .Take(numberOfPosts)
                .Select(group =>
                {
                    var numberOfLikes = group.Count();
                    var entry = group.First();
                    return _mappingService.MapLikesPerPost(numberOfLikes, entry.Post, entry.User);
                });
        }

        public override async Task PutUsersAsync(IEnumerable<UpdateUserModel> models)
        {
            _context.ChangeTracker.Clear();
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

        public override async Task DeleteUserPostsAsync(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                await _context.Posts.Where(x => x.UserId == id).ExecuteDeleteAsync();
            }
        }

        public override async Task ClearAllTablesAsync()
        {
            _context.ChangeTracker.Clear();
            await _context.Likes.ExecuteDeleteAsync();
            await _context.Posts.ExecuteDeleteAsync();
            await _context.Users.ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}