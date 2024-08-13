using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystem.Domain.Account;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace UserSystem.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, string>
    {
        Task<string> BulkInsertAsync();
    }
}