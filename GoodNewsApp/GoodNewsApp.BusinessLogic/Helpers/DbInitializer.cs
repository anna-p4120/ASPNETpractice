using GoodNewsApp.BusinessLogic.Services.UsersServices;
using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using GoodNewsApp.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Helpers
{
    public class DbInitializer
    {
        private readonly GoodNewsAppContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public DbInitializer(GoodNewsAppContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

        }

       /* public async Task InitializeWithUsersAndRolesAsync()
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


            if(!await _context.Users.AnyAsync(rl => rl.Name.Equals("Admin")))
            {
                string passwordHash, passwordSalt;
                PasswordManager.CreatePasswordHash("111", out passwordHash, out passwordSalt);
                var newUser = await _context.Users.AddAsync(new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    PasswordHash = passwordHash,
                    Email = "admin@mail.com",
                    PasswordSalt = passwordSalt

                });

                await _context.UserRoles.AddAsync(new UserRole()
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Entity.Id,
                    RoleId = (await _context.Roles.FirstOrDefaultAsync(rl => rl.Name.Equals("Admin"))).Id

                });

            }

            if (_context.ChangeTracker.HasChanges())
                await _context.SaveChangesAsync();

            
        }*/

        public void InitializeWithUsersAndRoles()

        {
            Role roleAdmin = _unitOfWork.RoleRepository.FindBy(u => u.Name.Equals("Admin")).FirstOrDefault();
            Role roleUser = _unitOfWork.RoleRepository.FindBy(u => u.Name.Equals("User")).FirstOrDefault();
            User admin = _unitOfWork.UserRepository.FindBy(u => u.Name.Equals("Admin")).FirstOrDefault();

            if (roleAdmin == null)
            {
                _unitOfWork.RoleRepository.Add(new Role() { Id = Guid.NewGuid(), Name = "Admin" });
            }

            if (roleUser == null)
            {
                _unitOfWork.RoleRepository.Add(new Role() { Id = Guid.NewGuid(), Name = "User" });
            }

            if (_context.ChangeTracker.HasChanges())
                 _context.SaveChanges();


            if (admin == null)
            {
                string passwordHash, passwordSalt;
                PasswordManager.CreatePasswordHash("111", out passwordHash, out passwordSalt);

                var newUser = _context.Users.Add(new User()
                {
                    Id = Guid.NewGuid(),
                    PasswordHash = passwordHash,
                    Name = "Admin",
                    Email = "admin@mail.com",
                    PasswordSalt = passwordSalt

                });

                _context.UserRoles.Add(new UserRole()
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Entity.Id,
                    RoleId = _unitOfWork.RoleRepository.FindBy(u => u.Name.Equals("Admin")).FirstOrDefault().Id

                });

            }

            if (_context.ChangeTracker.HasChanges())
                _context.SaveChanges();
            



        }
    }
}
