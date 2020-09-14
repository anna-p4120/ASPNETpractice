using GoodNewsApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Context
{
    public class DbInitializer
    {
        private readonly GoodNewsAppContext _context;

        public DbInitializer(GoodNewsAppContext context)
        {
            _context = context;
           
        }

        public async Task InitializeWithUsersAndRolesAsync()
        {
            if (!await _context.Roles.AnyAsync(rl =>rl.Name.Equals("Admin")))
            {
                await _context.Roles.AddAsync(new Role() { Id = Guid.NewGuid(), Name = "Admin" });
            }

            if (!await _context.Roles.AnyAsync(rl => rl.Name.Equals("User")))
            {
                await _context.Roles.AddAsync(new Role() { Id = Guid.NewGuid(), Name = "User" });
            }

            if (_context.ChangeTracker.HasChanges())
                await _context.SaveChangesAsync();


            if(!await _context.Users.AnyAsync(rl => rl.Name.Equals("Administrator")))
            {
                var newUser = await _context.Users.AddAsync(new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Administrator",
                    PasswordHash = "111",
                    Email = "admin@mail.com"

                } );

                await _context.UserRoles.AddAsync(new UserRole()
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Entity.Id,
                    RoleId = (await _context.Roles.FirstOrDefaultAsync(rl => rl.Name.Equals("Admin"))).Id

                });

            }

            if (_context.ChangeTracker.HasChanges())
                await _context.SaveChangesAsync();

            
        }
    }
}
