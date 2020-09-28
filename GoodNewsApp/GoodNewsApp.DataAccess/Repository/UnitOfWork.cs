using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GoodNewsAppContext _context;

        public IRepository<News> NewsRepository { get; }
        public IRepository<User> UserRepository { get; }

        public IRepository<UserRole> UserRoleRepository { get; }

        public IRepository<Role> RoleRepository { get; }

        public UnitOfWork(
            GoodNewsAppContext context, 
            IRepository<News> newsRepository,
            IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<Role> roleRepository)

        {
            _context = context;
            NewsRepository = newsRepository;
            UserRepository = userRepository;
            UserRoleRepository  = userRoleRepository;
            RoleRepository = roleRepository;

        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public int SaveChange()  
        {
                return _context.SaveChanges();

        }
    }
}
