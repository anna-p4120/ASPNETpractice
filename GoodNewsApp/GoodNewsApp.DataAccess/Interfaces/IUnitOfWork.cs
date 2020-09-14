using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}
