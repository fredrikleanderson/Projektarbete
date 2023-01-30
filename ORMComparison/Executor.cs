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
                _resultService.AddResults(await RetrieveAllUsersByIdAsync(execution.NumberOfUsersToGetById));
                _resultService.AddResults(await InsertPostsAsync(execution.NumberOfPostsPerUser));
                _resultService.AddResults(await RetrieveAllPostsAsync());
                _resultService.AddResults(await InsertLikesAsync(execution.NumberOfLikesPerUser));
                _resultService.AddResults(await RetrieveMostLikedPostsAsync(execution.NumberOfMostLikedPosts));
                _resultService.AddResults(await UpdateAllUsersAsync());
                _resultService.AddResults(await ClearDatabaseAsync());
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

        private async Task<IEnumerable<Result>> RetrieveAllUsersByIdAsync(int numberOfUsersToGetById)
        {
            var taskDescription = $"Retrieving {numberOfUsersToGetById} users by their ID";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.GetAllUsersByIdAsync(dapperUsers.Take(numberOfUsersToGetById)), taskDescription, ORMType.Dapper, MethodType.READ);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.GetAllUsersByIdAsync(efUsers.Take(numberOfUsersToGetById)), taskDescription, ORMType.EntityFrameWork, MethodType.READ);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> InsertPostsAsync(int numberOfPostsPerUser)
        {
            var taskDescrption = $"Inserting {numberOfPostsPerUser} post per user";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var dapperPosts = CreatePostModel.Factory.CreateRandomPosts(dapperUsers.ToArray(), numberOfPostsPerUser);
            var efPosts = CreatePostModel.Factory.CreateRandomPosts(efUsers.ToArray(), numberOfPostsPerUser);
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PostPostsAsync(dapperPosts), taskDescrption, ORMType.Dapper, MethodType.CREATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PostPostsAsync(efPosts), taskDescrption, ORMType.EntityFrameWork, MethodType.CREATE);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> RetrieveAllPostsAsync()
        {
            var taskDescrption = "Retrieving all posts";
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.GetAllPostsAsync(), taskDescrption, ORMType.Dapper, MethodType.READ);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.GetAllPostsAsync(), taskDescrption, ORMType.EntityFrameWork, MethodType.READ);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> InsertLikesAsync(int numberOfLikesPerUser)
        {
            var taskDescription = $"Inserting {numberOfLikesPerUser} like{(numberOfLikesPerUser > 1 ? "s" : "")} per user";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var dapperPosts = _dapperService.GetAllPostsAsync().Result.ToArray();
            var efPosts = _entityFrameworkService.GetAllPostsAsync().Result.ToArray();
            var dapperLikes = CreateLikeModel.Factory.CreateRandomLikes(dapperUsers, dapperPosts, numberOfLikesPerUser);
            var efLikes = CreateLikeModel.Factory.CreateRandomLikes(efUsers, efPosts, numberOfLikesPerUser);
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PostLikesAsync(dapperLikes), taskDescription, ORMType.Dapper, MethodType.CREATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PostLikesAsync(efLikes), taskDescription, ORMType.EntityFrameWork, MethodType.CREATE);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> RetrieveMostLikedPostsAsync(int numberOfMostLikedPosts)
        {
            var taskDescription = $"Retrieving {numberOfMostLikedPosts} most liked posts";
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.GetMostLikedPosts(numberOfMostLikedPosts), taskDescription, ORMType.Dapper, MethodType.READ);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.GetMostLikedPosts(numberOfMostLikedPosts), taskDescription, ORMType.EntityFrameWork, MethodType.READ);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> UpdateAllUsersAsync()
        {
            var taskDescription = "Updating all users";
            var dapperUsers = await _dapperService.GetAllUsersAsync();
            var efUsers = await _entityFrameworkService.GetAllUsersAsync();
            var updatedDapperUsers = UpdateUserModel.Factory.UpdateUsersRandomly(dapperUsers);
            var updatedEfUsers = UpdateUserModel.Factory.UpdateUsersRandomly(efUsers);
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.PutAllUsersAsync(updatedDapperUsers), taskDescription, ORMType.Dapper, MethodType.UPDATE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.PutAllUsersAsync(updatedEfUsers), taskDescription, ORMType.EntityFrameWork, MethodType.UPDATE);
            return new Result[] { dapperResult, efResult };
        }

        private async Task<IEnumerable<Result>> ClearDatabaseAsync()
        {
            var taskDescription = "Clearing database";
            var dapperResult = await _resultService.GetResultFromTaskAsync(_dapperService.ClearAllTablesAsync(), taskDescription, ORMType.Dapper, MethodType.DELETE);
            var efResult = await _resultService.GetResultFromTaskAsync(_entityFrameworkService.ClearAllTablesAsync(), taskDescription, ORMType.EntityFrameWork, MethodType.DELETE);
            return new Result[] { dapperResult, efResult };
        }
    }
}
