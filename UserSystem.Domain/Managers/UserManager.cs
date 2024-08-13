using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystem.Domain.Account;
using UserSystem.Domain.Repositories;
using UserSystem.Domain.Shared.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace UserSystem.Domain.Managers
{
    public class UserManager : DomainService
    {
        private readonly IUserRepository userRepository;

        public IRepository<User> UserRepo { get; set; }
        public IRepository<UserPassword> UserPasswordRepo { get; }
        public IRepository<Level> LevelRepo { get; }
        public IRepository<UserLevel> UserLevelRepo { get; }
        public IRepository<Role> RoleRepo { get; }
        public IRepository<UserRoleMapping> UserRoleMappingRepo { get; }

        public UserManager(
            IRepository<User> userRepo,
            IRepository<UserPassword> userPasswordRepo,
            IRepository<Level> levelRepo,
            IRepository<UserLevel> userLevelRepo,
            IRepository<Role> roleRepo,
            IRepository<UserRoleMapping> userRoleMappingRepo,
            IUserRepository userRepository
            )
        {
            UserRepo = userRepo;
            UserPasswordRepo = userPasswordRepo;
            LevelRepo = levelRepo;
            UserLevelRepo = userLevelRepo;
            RoleRepo = roleRepo;
            UserRoleMappingRepo = userRoleMappingRepo;
            this.userRepository = userRepository;
            this.userRepository = userRepository;
        }

        public async Task<bool> HasUserNoAsync(string userNo)
        {
            if (string.IsNullOrWhiteSpace(userNo))
            {
                throw new ArgumentNullException("userNo");
            }
            var res = !await UserRepo.AnyAsync(m => m.UserNo == userNo);
            return res;
        }

        public async Task<string> Test()
        {
            return await userRepository.BulkInsertAsync();
        }

        public async Task<User> GetUserAsync(string userId)
        {
            // var users = await UserRepo.GetListAsync(m => userId.Contains(m.Id));
            // var userLevelIds = users.Select(m => m.UserLevelId);
            // var userLevels = await UserLevelRepo.GetListAsync(m => userLevelIds.Contains(m.Id));
            // users.ForEach(m =>
            // {
            //     m.UserLevel = userLevels.FirstOrDefault(n => n.Id == m.UserLevelId);
            // });
            // return users;

            var user = await UserRepo.GetAsync(m => m.Id == userId);
            user.UserLevel = await UserLevelRepo.GetAsync(m => m.Id == user.UserLevelId);
            return user;
        }

        public async Task<User> GetLoginUser(string userNo, string password, LoginType loginType)
        {
            User? user = null;

            // user = loginType switch
            // {
            //     LoginType.Email => await UserRepo.GetAsync(m => m.Email == userNo),
            //     _ => await UserRepo.GetAsync(m => m.UserNo == userNo)
            // };

            user = await UserRepo.GetAsync(m => m.UserNo == userNo);

            if (user != null)
            {
                var userPassword = await UserPasswordRepo.GetAsync(m => m.UserId == user.Id && m.IsDisuse == "F");
                var isPwdRight = await IsPasswordRight(password, userPassword.Password);
                if (isPwdRight)
                {
                    return GetUserInfo(new List<User> { user }).Result.FirstOrDefault()!;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        private async Task<bool> IsPasswordRight(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        /// <summary>
        /// 获取完整用户信息
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private async Task<List<User>> GetUserInfo(List<User> users)
        {
            var userId = users.Select(m => m.Id);

            var userLevelIds = users.Select(m => m.UserLevelId);
            var userLevels = await UserLevelRepo.GetListAsync(m => userLevelIds.Contains(m.Id));

            var userRoles = UserRoleMappingRepo.GetListAsync(m => userId.ToList().Contains(m.UserId)).Result;

            var rolesId = userRoles.Select(m => m.RoleId).Distinct().ToList();

            var roles = await RoleRepo.GetListAsync(m => rolesId.Contains(m.Id));

            users.ForEach(m =>
            {
                m.UserLevel = userLevels.FirstOrDefault(n => n.Id == m.UserLevelId)!;

                var userRolesId = userRoles.FindAll(n => n.UserId == m.Id).Select(m => m.RoleId).Distinct().ToList();

                m.Roles = roles.FindAll(n => userRolesId.Contains(n.Id));
            });

            return users;
        }

        public async Task<bool> InsertAggregateAsync(User user)
        {
            try
            {
                user = await UserRepo.InsertAsync(user);
                user.UserPassword = await UserPasswordRepo.InsertAsync(user.UserPassword);
                user.UserLevel = await UserLevelRepo.InsertAsync(user.UserLevel);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}