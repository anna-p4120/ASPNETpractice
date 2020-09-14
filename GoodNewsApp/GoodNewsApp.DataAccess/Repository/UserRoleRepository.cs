using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Repository
{
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(GoodNewsAppContext context) : base(context)
        {
        }

        
    }
}
