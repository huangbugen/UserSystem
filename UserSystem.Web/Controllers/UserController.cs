using Microsoft.AspNetCore.Mvc;
using NLog;
using Qubiancheng.Abp.AspNet.JwtBearer;
using UserSystem.Application.Contract.UserApp;
using UserSystem.Application.Contract.UserApp.Dtos;
using UserSystem.Web.Filters;

namespace UserSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this._logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateRegisterInfoActionFilterAttribute))]
        public async Task<bool> RegisterUserAsync(UserCreateDto createInput)
        {
            var res = await _userService.RegisterUserAsync(createInput);
            return res;
        }

        [HttpPost("UploadFile")]
        public async Task<bool> UploadFile(IFormFile file)
        {
            string fileName = $"{Path.GetFileName(file.FileName)}";
            if (file.Length > 0)
            {
                var stream = file.GetAllBytes();
                await _userService.SaveAsync(fileName, stream);
                return true;
            }
            return false;
        }

        [HttpGet("Test")]
        public string Test()
        {
            _logger.LogError("错误");
            return "Test";
        }

        // [HttpGet]
        // public async Task<UserDto> GetUserById(string userId)
        // {
        //     return await _userService.GetUserDtoAsync(userId);
        // }

        [HttpGet("CheckLogin")]
        public async Task<ActionResult<string>> CheckLogin(string userNo, string password)
        {
            var token = await _userService.CheckLoginAsync(userNo, password);
            return token;
        }

        [HttpGet("RefreshToken")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var res = await _userService.RefreshToken();
            if (!res.isSuccess)
            {
                return Unauthorized(res.token);
            }
            else
            {
                return res.token;
            }
        }
    }
}