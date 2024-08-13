using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystem.Domain.Account;

namespace UserSystem.Domain.Shared.SharedModels
{
    public class UserRefreshToken
    {
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }

    public class UserRefreshTokenKey
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        public override string ToString()
        {
            return $"{UserId}_{Token}";
        }
    }
}