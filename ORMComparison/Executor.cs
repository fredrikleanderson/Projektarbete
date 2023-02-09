using Models;
using Services.Helpers;
using Services.Interfaces;
using static Services.Helpers.Delegates;
using static Services.Providers.ResultService;

namespace ORMComparison
{

    public class Executor: IExecutor
    {
        private readonly IResultService _resultService;
        private readonly IDataService _dapperService;
        private readonly IDataService _entityFrameworkService;

        public Executor(IResultService resultService, DataServiceResolver resolver)
        {
            _resultService = resultService;
            _dapperService = resolver(ORMType.Dapper);
            _entityFrameworkService = resolver(ORMType.EntityFrameWork);
        }

        public async Task RunAsync(Execution execution)
        {
            await _dapperService.ClearAllTablesAsync();
            await _entityFrameworkService.ClearAllTablesAsync();

            for (int i = 0; i < execution.NumberOfRuns + 1; i++)
            {
                _resultService.AddResults(await InsertUsersAsync(execution.NumberOfUsers));
                _resultService.AddResults(await RetrieveAllUsersAsync());
                _resultService.AddResults(await InsertPostsAsync(execution.NumberOfPostingUsers, execution.NumberOfPostsPerUser));
                _resultService.AddResults(await RetrieveUserPagesAsync(execution.NumberOfUserPages));
                _resultService.AddResults(await InsertLikesAsync(execution.NumberOfLikingUsers, execution.NumberOfLikesPerUser));
                _resultService.AddResults(await RetrieveMostLikedPostsAsync(execution.NumberOfMostLikedPosts));
                _resultService.AddResults(await UpdateUsersAsync(execution.NumberOfUsersToUpdate));
                _resultService.AddResults(await RemoveUserPosts(execution.NumberOfUsersDeletingTheirPosts));
            }

            _resultService.PrintResults();
        }

        private async Task<IEnumerable<Result>> InsertUsersAsync(int numberOfUsers)
        {
            var taskDescription = $"Inserting {numberOfUsers} users";
            var newUsers = CreateUserModel.Factory.GetRandomUsers(numberOfUsers);
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PostUsersAsync(newUsers), taskDescription, ORMType.Dapper, MethodType.CREATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PostUsersAsync(newUsers), taskDescription, ORMType.EntityFrameWork, MethodType.CREATE);
            return new Result[] { dapperResult, efResult }; 
        }

        private async Task<IEnumerable<Result>> RetrieveAllUsersAsync()
        {
            var taskDescription = "Retrieving all users";
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.GetAllUsersAsync(), taskDescription, ORMType.Dapper, MethodType.READ);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.GetAllUsersAsync(), taskDescription, ORMType.EntityFrameWork, MethodType.READ);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> InsertPostsAsync(int numberOfPostingUsers, int numberOfPostsPerUser)
        {
            var taskDescrption = $"Inserting {numberOfPostsPerUser} posts per user for {numberOfPostingUsers} users";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var dapperPosts = CreatePostModel.Factory.CreateRandomPosts(dapperUsers.Take(numberOfPostingUsers).ToArray(), numberOfPostsPerUser);
            var efPosts = CreatePostModel.Factory.CreateRandomPosts(efUsers.Take(numberOfPostingUsers).ToArray(), numberOfPostsPerUser);
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PostPostsAsync(dapperPosts), taskDescrption, ORMType.Dapper, MethodType.CREATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PostPostsAsync(efPosts), taskDescrption, ORMType.EntityFrameWork, MethodType.CREATE);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> RetrieveUserPagesAsync(int numberOfUserPages)
        {
            var taskDescription = $"Retrieving {numberOfUserPages} user profiles";
            var dapperPosts = _dapperService.GetAllPostsAsync().Result.ToArray();
            var efPosts = _entityFrameworkService.GetAllPostsAsync().Result.ToArray();
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.GetUserPages(dapperPosts.Take(numberOfUserPages).Select(post => post.User.Id)), taskDescription, ORMType.Dapper, MethodType.READ);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.GetUserPages(efPosts.Take(numberOfUserPages).Select(post => post.User.Id)), taskDescription, ORMType.EntityFrameWork, MethodType.READ);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> InsertLikesAsync(int numberOfLikingUsers, int numberOfLikesPerUser)
        {
            var taskDescription = $"Inserting {numberOfLikesPerUser} like{(numberOfLikesPerUser > 1 ? "s" : "")} per user for {numberOfLikingUsers} users";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var dapperPosts = _dapperService.GetAllPostsAsync().Result.ToArray();
            var efPosts = _entityFrameworkService.GetAllPostsAsync().Result.ToArray();
            var dapperLikes = CreateLikeModel.Factory.CreateRandomLikes(dapperUsers.Take(numberOfLikingUsers), dapperPosts, numberOfLikesPerUser);
            var efLikes = CreateLikeModel.Factory.CreateRandomLikes(efUsers.Take(numberOfLikingUsers), efPosts, numberOfLikesPerUser);
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PostLikesAsync(dapperLikes), taskDescription, ORMType.Dapper, MethodType.CREATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PostLikesAsync(efLikes), taskDescription, ORMType.EntityFrameWork, MethodType.CREATE);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> RetrieveMostLikedPostsAsync(int numberOfMostLikedPosts)
        {
            var taskDescription = $"Retrieving the {numberOfMostLikedPosts} most liked posts";
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.GetMostLikedPosts(numberOfMostLikedPosts), taskDescription, ORMType.Dapper, MethodType.READ);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.GetMostLikedPosts(numberOfMostLikedPosts), taskDescription, ORMType.EntityFrameWork, MethodType.READ);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> UpdateUsersAsync(int numberOfUsersToUpdate)
        {
            var taskDescription = $"Updating {numberOfUsersToUpdate} users";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var updatedDapperUsers = UpdateUserModel.Factory.UpdateUsersRandomly(dapperUsers.Take(numberOfUsersToUpdate));
            var updatedEfUsers = UpdateUserModel.Factory.UpdateUsersRandomly(efUsers.Take(numberOfUsersToUpdate));
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PutUsersAsync(updatedDapperUsers), taskDescription, ORMType.Dapper, MethodType.UPDATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PutUsersAsync(updatedEfUsers), taskDescription, ORMType.EntityFrameWork, MethodType.UPDATE);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> RemoveUserPosts(int numberOfUsersDeletingTheirPosts)
        {
            var taskDescription = $"Deleting all posts by {numberOfUsersDeletingTheirPosts} users";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.DeleteUserPostsAsync(dapperUsers.Select(user => user.Id)), taskDescription, ORMType.Dapper, MethodType.DELETE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.DeleteUserPostsAsync(efUsers.Select(user => user.Id)), taskDescription, ORMType.EntityFrameWork, MethodType.DELETE);
            return new Result[] { dapperResult, efResult };
        }
    }
}