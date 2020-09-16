using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoodNewsApp.DataAccess.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        //public DateTime UserRegistrationDate { get; set; }

        public virtual IEnumerable<Comment> Comment { get; set; }
        public virtual IEnumerable<UserRole> UserRole { get; set; }

    }
}
