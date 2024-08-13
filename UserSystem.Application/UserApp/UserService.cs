using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Qubiancheng.Abp.AspNet.JwtBearer;
using UserSystem.Application.Contract.UserApp;
using UserSystem.Application.Contract.UserApp.Dtos;
using UserSystem.Domain.Account;
using UserSystem.Domain.Managers;
using UserSystem.Domain.Shared.Containers;
using UserSystem.Domain.Shared.Enums;
using UserSystem.Domain.Shared.SharedModels;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;

namespace UserSystem.Application.UserApp
{
    public class UserService : ApplicationService, IUserService
    {
        private readonly UserManager _userManager;
        private readonly LevelManager _levelManager;
        private readonly TokenCreateModel _tokenCreateModel;
        private readonly IDistributedCache<UserRefreshToken> _cache;
        private readonly IBlobContainer<FileContainer> _blobContainer;

        public HttpContext HttpContext { get; set; }

        public UserService(
            UserManager userManager,
            LevelManager levelManager,
            TokenCreateModel tokenCreateModel,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache<UserRefreshToken> cache,
            IBlobContainer<FileContainer> blobContainer)
        {
            _userManager = userManager;
            _levelManager = levelManager;
            _tokenCreateModel = tokenCreateModel;
            _cache = cache;
            _blobContainer = blobContainer;
            HttpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<bool> RegisterUserAsync(UserCreateDto createInput)
        {
            // 通过领域服务判断是否已存在UserNo
            var res = await _userManager.HasUserNoAsync(createInput.UserNo);
            if (res)
            {
                // 设置用户
                var user = ObjectMapper.Map<UserCreateDto, User>(createInput);
                // 获取初始化等级
                var levels = await _levelManager.GetTop2LevelAsync();
                var levelNow = levels.FirstOrDefault();
                var levelNext = levels[1];
                // 初始化创建用的信息
                user.CreateUser(levelNow!);
                // 将信息存入数据库
                // user = await _userManager.UserRepo.InsertAsync(user);
                // user.UserPassword = await _userManager.UserPasswordRepo.InsertAsync(user.UserPassword);
                // user.UserLevel = await _userManager.UserLevelRepo.InsertAsync(user.UserLevel);
                await _userManager.InsertAggregateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<UserDto> GetUserDtoAsync(string userId)
        {
            var user = await _userManager.GetUserAsync(userId);
            var dto = ObjectMapper.Map(user, new List<UserDto>());
            return dto.FirstOrDefault();
        }

        public async Task<string> CheckLoginAsync(string userNo, string password)
        {
            var loginType = GetLoginType(userNo);
            string token = string.Empty;
            var user = await _userManager.GetLoginUser(userNo, password, loginType);
            if (user != null)
            {
                token = _tokenCreateModel.GetToken(user.Id, new Claim("userName", user.UserName), new Claim("headUrl", user.HeadUrl));
                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken, user);
            }
            return token;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        private async void SetRefreshToken(RefreshToken refreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };

            HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            UserRefreshToken userRefreshToken = new UserRefreshToken
            {
                User = user,
                Token = refreshToken.Token,
                TokenCreated = refreshToken.Created,
                TokenExpires = refreshToken.Expires
            };

            await _cache.SetAsync(refreshToken.Token, userRefreshToken);
        }

        public async Task<(string token, bool isSuccess)> RefreshToken()
        {
            var cookieRefreshToken = HttpContext.Request.Cookies["refreshToken"];
            var value = await _cache.GetAsync(cookieRefreshToken!);
            if (value != null)
            {
                var userRefreshToken = value;
                if (userRefreshToken.Token != cookieRefreshToken)
                {
                    return ("refreshToken 验证失败", false);
                }
                else if (userRefreshToken.TokenExpires < DateTime.Now)
                {
                    return ("refreshToken 已过期", false);
                }
                var token = _tokenCreateModel.GetToken(userRefreshToken.User.Id);
                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken, userRefreshToken.User);
                return (token, true);
            }
            return ("当前用户不存在", false);
        }

        private LoginType GetLoginType(string loginNo)
        {
            Regex regexEmail = new Regex("/^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");

            if (regexEmail.IsMatch(loginNo))
            {
                return LoginType.Email;
            }
            return LoginType.UserNo;
        }

        public async Task SaveAsync(string name, Byte[] file)
        {
            await _blobContainer.SaveAsync(name, file, true);
        }
    }
}