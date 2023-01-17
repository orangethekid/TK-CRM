using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Server.Data;
using TakraonlineCRM.Server.Models;
using TakraonlineCRM.Shared.WebSites;

namespace TakraonlineCRM.Server
{
    public class Seed
    {
        public static async Task SeedData( ApplicationDBContext context, UserManager<ApplicationUser> userManager )
        {
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = "admin",
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole
                    {
                        Id = "subadmin",
                        Name = "SubAdmin",
                        NormalizedName = "SUBADMIN",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole
                    {
                        Id = "customerservice",
                        Name = "CustomerService",
                        NormalizedName = "CUSTOMERSERVICE",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole
                    {
                        Id = "sale",
                        Name = "Sale",
                        NormalizedName = "SALE",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole
                    {
                        Id = "programer",
                        Name = "Programer",
                        NormalizedName = "PROGRAMER",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new IdentityRole
                    {
                        Id = "graphic",
                        Name = "Graphic",
                        NormalizedName = "GRAPHIC",
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                };
                context.Roles.AddRange( roles );
                context.SaveChanges();
            }

            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        DisplayName = "Admin",
                        UserName = "admin",
                        Email = "admin@127.0.0.1"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Sub Admin",
                        UserName = "subadmin",
                        Email = "subadmin@127.0.0.1"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Customer Service",
                        UserName = "customerservice",
                        Email = "customerservice@127.0.0.1"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Sale",
                        UserName = "sale",
                        Email = "sale@127.0.0.1"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Programer",
                        UserName = "programer",
                        Email = "programer@127.0.0.1"
                    },
                    new ApplicationUser
                    {
                        DisplayName = "Graphic",
                        UserName = "graphic",
                        Email = "graphic@127.0.0.1"
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync( user, "Pa$$w0rd" );
                    switch (user.UserName)
                    {
                        case "admin":
                            await userManager.AddToRoleAsync( user, "admin" );
                            break;
                        case "subadmin":
                            await userManager.AddToRoleAsync( user, "subadmin" );
                            break;
                        case "customerservice":
                            await userManager.AddToRoleAsync( user, "customerservice" );
                            break;
                        case "programer":
                            await userManager.AddToRoleAsync( user, "programer" );
                            break;
                        case "graphic":
                            await userManager.AddToRoleAsync( user, "graphic" );
                            break;
                        default:
                            await userManager.AddToRoleAsync( user, "sale" );
                            break;
                    }
                }
            }
        }
    }
}
