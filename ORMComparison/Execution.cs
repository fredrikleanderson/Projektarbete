using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMComparison
{
    public class Execution
    {
        public int NumberOfRuns { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfPostingUsers { get; set; }
        public int NumberOfPostsPerUser { get; set; }
        public int NumberOfUserPages { get; set; }
        public int NumberOfLikingUsers { get; set; }
        public int NumberOfLikesPerUser { get; set; }
        public int NumberOfMostLikedPosts { get; set; }
        public int NumberOfUsersToUpdate { get; set; }
        public int NumberOfUsersDeletingTheirPosts { get; set; }
    }
}
