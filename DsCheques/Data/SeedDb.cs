using DsCheques.Data.Entities;
using DsCheques.Helpers;
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
        private readonly IUserHelper userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            var user = await this.userHelper.GetUserByEmailAsync("dagsis@dagsis.com.ar");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Carlos",
                    LastName = "D Agostino",
                    Email = "dagsis@dagsis.com.ar",
                    UserName = "dagsis@dagsis.com.ar"
                };

                var result = await this.userHelper.AddUserAsync (user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

        }

    }
}
