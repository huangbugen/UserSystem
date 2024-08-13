using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace UserSystem.Domain.Account
{
    public class UserPassword : Entity<string>, IHasCreationTime
    {
        public string Password { get; set; }
        public string UserId { get; set; }
        public DateTime CreationTime { get; set; }
        // T/F/....
        public string IsDisuse { get; set; }

        public UserPassword()
        {
            Id = Guid.NewGuid().ToString("N");
            Password = BCrypt.Net.BCrypt.HashPassword("123456");
        }
    }
}