using GoodNewsApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        public IRepository<News> NewsRepository { get; }
        public IRepository<User> UserRepository { get; }

        public IRepository<UserRole> UserRoleRepository { get; }

        public IRepository<Role> RoleRepository { get; }

        Task<int> SaveChangeAsync();
    }
}
