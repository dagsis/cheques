using DsCheques.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data
{
    public class SeedDb
    {
        private readonly DataContext context;
        private readonly UserManager<User> userManager;

        public SeedDb(DataContext context, UserManager<User>userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var user = await this.userManager.FindByEmailAsync("dagsis@dagsis.com.ar");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Carlos",
                    LastName = "D Agostino",
                    Email = "dagsis@dagsis.com.ar",
                    UserName = "dagsis@dagsis.com.ar"
                };

                var result = await this.userManager.CreateAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

        }

    }
}
