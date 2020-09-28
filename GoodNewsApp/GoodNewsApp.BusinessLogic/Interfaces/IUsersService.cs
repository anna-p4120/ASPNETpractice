using GoodNewsApp.BusinessLogic.Services.UsersServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Interfaces
{
    public interface IUsersService
    {
        Task<UserDTO> GetUserByEmailAsync(string email);

        List<Guid> GetRoleIdByUserId(Guid id);

        Task<CreatedUserRole> CreateUserRoleAsync(UserDTO userDTO, params string[] userRoles);

    }
}
