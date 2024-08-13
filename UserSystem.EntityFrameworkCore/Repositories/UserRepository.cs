using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystem.Domain.Account;
using UserSystem.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace UserSystem.EntityFrameworkCore.Repositories
{
    public class UserRepository : EfCoreRepository<UserSystemDbContext, User, string>, IUserRepository
    {
        public UserRepository(IDbContextProvider<UserSystemDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<string> BulkInsertAsync()
        {
            var dbContext = await GetDbContextAsync();


            return "黄步根";
        }
    }
}