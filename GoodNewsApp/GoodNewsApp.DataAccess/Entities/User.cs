using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        //public DateTime UserRegistrationDate { get; set; }

        public virtual IEnumerable<Comment> Comment { get; set; }
        public virtual IEnumerable<UserRole> UserRole { get; set; }

    }
}
