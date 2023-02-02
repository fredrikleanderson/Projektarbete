using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateUserModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public override string ToString()
        {
            return $"{FirstName}-{LastName}-{Email}-{Password}";
        }

        public static class Factory
        {
            private static Random random = new();
            private static string[] firstNames = "Erik Ola Emma Petra Conny Johan Bella Anna Pierre Lars Kenny Klara Dagny Stellan Albert Lova Juni Bastian".Split();
            private static string[] lastNames = "Höglund Andersson Emilsson Grönqvist Ståhl Bagelin Thyr Pettersson Törmänen Olausson Gran Grön Edlund Albrektsson".Split();
            private static string[] emails = "gmail.com yahoo.com teknikhogskolan.se soderhamn.se bing.com microsoft.com timcorey.us".Split();
            private static CreateUserModel GetRandomUser()
            {                 
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var email = emails[random.Next(emails.Length)];

                return new CreateUserModel()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@{email}",
                    Password = $"{firstName}_banan666"
                };
            }

            public static IEnumerable<CreateUserModel> GetRandomUsers(int quantity) 
            {
                return Enumerable.Range(0, quantity).Select(element => GetRandomUser());         
            }

        }
    }
}
