using GoodNewsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GoodNewsApp.Models.Context
{
    public class GoodNewsAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public GoodNewsAppContext(DbContextOptions<GoodNewsAppContext> options): base (options)
        {
            Database.EnsureCreated();
        }


    }
}
