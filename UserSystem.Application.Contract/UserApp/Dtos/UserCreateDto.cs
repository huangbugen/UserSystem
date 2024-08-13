using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystem.Application.Contract.UserApp.Dtos
{
    public class UserCreateDto
    {
        public string UserNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}