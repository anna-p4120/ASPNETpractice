using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Entities
{
    public class UserRole : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }

    }
}
