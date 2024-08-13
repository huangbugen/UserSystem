using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserSystem.Domain.Account;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace UserSystem.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class UserSystemDbContext : AbpDbContext<UserSystemDbContext>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<UserLevel> UserLevel { get; set; }
        public DbSet<UserPassword> UserPassword { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }


        public UserSystemDbContext(DbContextOptions<UserSystemDbContext> options) : base(options)
        {
        }
    }
}