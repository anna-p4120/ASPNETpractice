using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Entities
{
    public class Role : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public  IEnumerable<UserRole> UserRoles { get; set; }


    }

   
}
