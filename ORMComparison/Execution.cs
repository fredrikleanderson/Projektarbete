﻿using System;
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
        public int NumberOfPostsPerUser { get; set; }
        public int NumberOfUsersToGetById { get; set; }
        public int NumberOfLikesPerUser { get; set; }
        public int NumberOfMostLikedPosts { get; set; }
    }
}