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

        public NewsRepository NewsRepository { get; }
        public UserRepository UserRepository { get; }

        public UserRoleRepository UserRoleRepository { get; }

        public RoleRepository RoleRepository { get; }

        public UnitOfWork(GoodNewsAppContext context, NewsRepository newsRepository, UserRepository userRepository, UserRoleRepository userRoleRepository,  RoleRepository roleRepository)
        {
            _context = context;
            NewsRepository = newsRepository;
            UserRepository = userRepository;
            UserRoleRepository  = userRoleRepository;
            RoleRepository = roleRepository;

        }

        //GoodNewsAppContext IUnitOfWork.Context => throw new NotImplementedException();

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
