using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IQueryService
    {
        string InsertUsers(CreateUserModel[] models);
        string InsertPosts(CreatePostModel[] models);
        string InsertLikes(CreateLikeModel[] models);
        string SelectAllUsers();
        string SelectUserById(UserModel model);
        string SelectAllPosts();
        string SelectMostLikedPosts(int quantity);
        string UpdateSingleUser(UpdateUserModel model);
        string ClearDatabase();
    }
}
