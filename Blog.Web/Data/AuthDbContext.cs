using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //seeding roles (user, admin, superAdmin)
            var adminRoleId = "97956be1-7c4f-4ab1-ab07-d917abb081d2";
            var superAdminRoleId = "db744dc0-08c9-4688-82ca-bc9b2a7634af";
            var userRoleId = "50950b2f-2d9b-4fe4-85bd-55cfa0061430";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId

                }
            };

            //inserting data to data base
            builder.Entity<IdentityRole>().HasData(roles);

            // seed superAdminUser
            var superAdminId = "51da03b5-2a86-40bc-9201-8b2ce75dcdc9";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@blog.com",
                Email = "superadmin@blog.com",
                NormalizedEmail = "superadmin@blog.com".ToUpper(),
                NormalizedUserName = "superadmin@blog.com".ToUpper(),
                Id = superAdminId
            };

            //creating password for this user
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "SuperAdmin123");

            //inserting this user to db
            builder.Entity<IdentityUser>().HasData(superAdminUser);


            //add all roles to superAdminUser, seeding table Roles-Users
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminUser.Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminUser.Id
                }
            };

            //inserting user-roles enities to db
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }
    }
}
