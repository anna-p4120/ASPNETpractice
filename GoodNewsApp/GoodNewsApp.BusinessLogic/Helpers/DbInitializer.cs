using GoodNewsApp.BusinessLogic.Services.UsersServices;
using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using GoodNewsApp.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Helpers
{
    public class DbInitializer
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        public DbInitializer(GoodNewsAppContext context, IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;

        }

       
        public void InitializeWithUsersAndRoles()

        {
            Role roleAdmin = _unitOfWork.RoleRepository.FindBy(u => u.Name.Equals(DefaultRolesList.Admin.ToString())).FirstOrDefault();

            Role roleUser = _unitOfWork.RoleRepository.FindBy(u => u.Name.Equals(DefaultRolesList.User.ToString())).FirstOrDefault();
            

            if (roleAdmin == null)
            {
                roleAdmin = new Role() { Id = Guid.NewGuid(), Name = DefaultRolesList.Admin.ToString() };

                _unitOfWork.RoleRepository.Add(roleAdmin);
                _unitOfWork.SaveChange();
            }

            if (roleUser == null)
            {
                roleUser = new Role() { Id = Guid.NewGuid(), Name = DefaultRolesList.User.ToString() };

                _unitOfWork.RoleRepository.Add(roleUser);
                _unitOfWork.SaveChange();
            }

            UserRole adminUserRole = _unitOfWork.UserRoleRepository.FindBy(u => u.RoleId.Equals(roleAdmin.Id)).FirstOrDefault();//

            if (adminUserRole == null)
            {
                string passwordHash, passwordSalt;

                PasswordManager.CreatePasswordHash(_appSettings.InitialAdminPassword, out passwordHash, out passwordSalt);

                User newAdmin = new User()
                {
                    Id = Guid.NewGuid(),
                    PasswordHash = passwordHash,
                    Name = _appSettings.InitialAdminName,
                    Email = _appSettings.InitialAdminEmail,
                    PasswordSalt = passwordSalt
 
                }; 

                adminUserRole = new UserRole()
                {
                    Id = Guid.NewGuid(),
                    UserId = newAdmin.Id,
                    RoleId = roleAdmin.Id,
                    RegistrationDate = DateTime.Now,

                };

                _unitOfWork.UserRepository.Add(newAdmin);

                _unitOfWork.UserRoleRepository.Add(adminUserRole);

                _unitOfWork.SaveChange();

            }

        }
    }

    public enum DefaultRolesList
    {
        Admin,
        User
    }
}
