using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.Models.Entities
{
    public class UserRole
    {
        public Guid Id { get; set; }

        public Guid UsersId { get; set; }
        public Guid RolesId { get; set; }

        public DateTime RegistrationDate { get; set; }


        
    }
}
