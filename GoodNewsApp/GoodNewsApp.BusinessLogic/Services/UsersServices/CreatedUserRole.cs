using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.BusinessLogic.Services.UsersServices
{
    public class CreatedUserRole
    {
        public string Email { get; set; }

        public List<Guid> UserRolesID { get; set; }
    }
}
