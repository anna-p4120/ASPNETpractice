using AutoMapper;
using GoodNewsApp.BusinessLogic.Helpers;
using GoodNewsApp.BusinessLogic.Interfaces;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Services.UsersServices
{
    public class UsersService:IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

      
        public async Task<UserDTO> GetUserByEmailAsync(string email) //, CancellationToken token
        {

            User userWithEmail = await _unitOfWork.UserRepository.FindBy(u => u.Email == email).FirstOrDefaultAsync();
            UserDTO userDTOWithEmail = _mapper.Map<UserDTO>(userWithEmail);
            return userDTOWithEmail;

        }
        public List<Guid> GetRoleIdByUserId(Guid id) //, CancellationToken token
        {
            List<Guid> roleOfUserId = _unitOfWork.UserRoleRepository.FindBy(u => u.UserId == id).Select(r => r.Id).ToList();
            
            return roleOfUserId;

        }

        //TODO cancellation token
        public async Task<CreatedUserRole> CreateUserRoleAsync(UserDTO userDTO, params string[] userRoles)
        {

            User newUserToAdd = new User()
            {
                Id = Guid.NewGuid(),
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
                PasswordSalt = userDTO.PasswordSalt
            };

            await _unitOfWork.UserRepository.AddAsync(newUserToAdd);


            List<Guid> rolesOfNewUserId = new List<Guid>() { };


            if (userRoles.Length == 0)
            {
                string defaultRole = DefaultRolesList.User.ToString();

                Role defaultRoleFromDB = await _unitOfWork.RoleRepository.FindBy(u => u.Name == defaultRole).FirstOrDefaultAsync();

                if (defaultRoleFromDB != null)
                {
                    Guid defaultRoleId = defaultRoleFromDB.Id;

                    UserRole newUserRoleToAdd = new UserRole()
                    {
                        Id = Guid.NewGuid(),
                        UserId = newUserToAdd.Id,
                        RoleId = defaultRoleId,
                        RegistrationDate = DateTime.Now,
                    };

                    await _unitOfWork.UserRoleRepository.AddAsync(newUserRoleToAdd);

                    rolesOfNewUserId.Add(newUserRoleToAdd.RoleId);

                }

                else
                {
                    throw new Exception ("Роль "+defaultRole+" не определена");
                }
            
            }
            else
            {
                //???endless number of roles //Cancelation token??? //just for those ones from DB

                for (int i = 0; i < userRoles.Length; i++)
                {
                    Role currentRole = await _unitOfWork.RoleRepository.FindBy(u => u.Name == userRoles[i]).FirstOrDefaultAsync();

                    if (currentRole != null)
                    {
                        Guid defaultRoleId = currentRole.Id;

                        UserRole newUserRoleToAdd = new UserRole()
                        {
                            Id = Guid.NewGuid(),
                            UserId = newUserToAdd.Id,
                            RoleId = defaultRoleId,
                            RegistrationDate = DateTime.Now,

                        };
                        rolesOfNewUserId.Add(newUserRoleToAdd.RoleId);

                        await _unitOfWork.UserRoleRepository.AddAsync(newUserRoleToAdd);

                    }
                    else
                    {
                        throw new Exception("Роль " + userRoles[i] + " не определена");

                    }
                }
                


            }
  

            await _unitOfWork.SaveChangeAsync();

            CreatedUserRole newCreatedUserRole = new CreatedUserRole()
            {
                Email = newUserToAdd.Email,
                UserRolesID = rolesOfNewUserId
            };

            return newCreatedUserRole; 
        }
        
        

    }
}
