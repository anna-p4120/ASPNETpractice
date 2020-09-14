using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Repository
{
    public class NewsRepository : Repository<News>
    {
        public NewsRepository(GoodNewsAppContext context) : base(context)
        {
        }

    }
}
