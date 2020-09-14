using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Entities
{
    public class Role : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<UserRole> UserRole { get; set; }


    }

    //?Configuration  ?сlass
    enum RolesList
    {
        Admin,
        User
    }
}
