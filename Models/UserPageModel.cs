using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserPageModel
    {
        public UserModel User { get; set; } = null!;
        public IEnumerable<PostModel> Posts { get; set; } = null!;
    }
}
