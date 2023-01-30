using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LikesPerPostModel
    {
        public int Likes { get; set; }
        public PostModel Post { get; set; } = null!;
    }
}