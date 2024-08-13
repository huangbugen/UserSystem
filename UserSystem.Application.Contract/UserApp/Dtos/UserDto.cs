using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace UserSystem.Application.Contract.UserApp.Dtos
{
    public class UserDto : EntityDto<string>
    {
        public string UserName { get; set; }
        public string UserNo { get; set; }
        public string HeadUrl { get; set; }
        public string UserLevelId { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string ActiveCode { get; set; }
        public UserLevelDto UserLevel { get; set; }
    }
}