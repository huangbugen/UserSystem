using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystem.Domain.Shared.SharedModels
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
    }
}