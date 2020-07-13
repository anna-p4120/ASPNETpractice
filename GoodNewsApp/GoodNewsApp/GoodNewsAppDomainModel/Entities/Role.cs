using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.GoodNewsAppDomainModel.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserRole> UserRole { get; set; }


    }
}
