using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace UserSystem.Domain.Account
{
    public class UserLevel : Entity<string>
    {
        public int Integral { get; set; }
        public string LevelId { get; set; }

        public UserLevel()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
}