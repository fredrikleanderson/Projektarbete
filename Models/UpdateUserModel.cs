using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public string NewFirstName { get; set; } = null!;
        public string NewLastName { get; set; } = null!;
        public string NewEmail { get; set; } = null!;
        public string NewPassword { get; set; } = null!;

        public static class Factory
        {
            private static Random random = new();
            private static string[] firstNames = "Jeff Steven Bonnie Patty Chris John Belle Anne Bradley Larry Josh Claire Daphne Seb Andy Liza June Jeff".Split();
            private static string[] lastNames = "Smith Anderson Miller Black Steele Baker Tiller Peterson Clooney Davies Taylor Evans Jones Williams".Split();
            private static string[] emails = "gmail.com yahoo.com teknikhogskolan.se soderhamn.se bing.com microsoft.com timcorey.us".Split();
            private static UpdateUserModel UpdateUserRandomly(UserModel model)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var email = emails[random.Next(emails.Length)];

                return new UpdateUserModel()
                {
                    Id = model.Id,
                    NewFirstName = firstName,
                    NewLastName = lastName,
                    NewEmail = $"{firstName.ToLower()}.{lastName.ToLower()}@{email}",
                    NewPassword = $"{firstName}_kiwi999"
                };
            }

            public static IEnumerable<UpdateUserModel> UpdateUsersRandomly(IEnumerable<UserModel> models)
            {
                return models.Select(model => UpdateUserRandomly(model));
            }
        }

    }
}
