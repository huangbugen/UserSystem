using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace UserSystem.Domain.Account
{
    public class UserRoleMapping : Entity<long>
    {
        public string UserId { get; set; }
        public long RoleId { get; set; }
    }
}