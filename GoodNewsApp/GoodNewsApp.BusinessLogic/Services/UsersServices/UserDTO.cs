using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.BusinessLogic.Services.UsersServices
{
    public class UserDTO

    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

       
    }

}
