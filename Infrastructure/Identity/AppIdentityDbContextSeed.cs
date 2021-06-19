using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                Console.WriteLine("User Not exist");
                var user = new AppUser
                {
                     UserName="bob@test.com",
                     Email="bob@test.com",
                     DisplayName="Ope",
                     Address = new Address
                     {
                         Firstname="Opeyemi",
                         Lastname="Adenuga",
                         Street="10 Downing street",
                         City="New Your",
                         State="Lagos"
                     }
                };         

                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
            else 
            {           
                Console.WriteLine("User  exist");
            }

        }
    }
}