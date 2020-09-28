using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace GoodNewsApp.DataAccess.Entities
{
    public class User : IEntity
    {
        [Required]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
       
        [Required]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        //public DateTime UserRegistrationDate { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        
        
        public IEnumerable<UserRole> UserRoles { get; set; }

    }
}
