using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace UserSystem.Domain.Account
{
    public class User : AggregateRoot<string>, IHasCreationTime
    {
        public string UserName { get; set; }
        public string UserNo { get; set; }
        public string HeadUrl { get; set; }
        public string UserLevelId { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string ActiveCode { get; set; }

        [NotMapped]
        public List<Role> Roles { get; set; }

        [NotMapped]
        public UserLevel UserLevel { get; set; }

        [NotMapped]
        public UserPassword UserPassword { get; set; }

        public User()
        {

        }
        /// <summary>
        /// 初始化用户信息
        /// </summary>
        public User(string userId) : base(userId)
        {
            // this.Id = Guid.NewGuid().ToString("N");
            this.CreationTime = DateTime.Now;
            this.Status = 0;
            this.ActiveCode = Guid.NewGuid().ToString("N");
            RandomHeadUrl();
        }
        /// <summary>
        /// 创建随机头像
        /// </summary>
        public void RandomHeadUrl()
        {
            this.HeadUrl = $"/img/head/head({new Random().Next(1, 9)}).png";
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="levelNow"></param>
        public void CreateUser(Level levelNow)
        {
            SetPassword();
            SetUserLevel(levelNow);
        }
        /// <summary>
        /// 配置用户密码
        /// </summary>
        public void SetPassword()
        {
            this.UserPassword = new();
            this.UserPassword.CreationTime = this.CreationTime;
            this.UserPassword.UserId = this.Id;
            this.UserPassword.IsDisuse = "F";
        }
        /// <summary>
        /// 配置用户等级信息
        /// </summary>
        /// <param name="levelNow"></param>
        public void SetUserLevel(Level levelNow)
        {
            this.UserLevel = new();
            this.UserLevelId = this.UserLevel.Id;
            this.UserLevel.Integral = levelNow.NeedIntegral;
            this.UserLevel.LevelId = levelNow.Id;
        }
    }
}