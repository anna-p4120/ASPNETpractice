using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Repository
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(GoodNewsAppContext context) : base(context)
        {
        }

        
    }
}
