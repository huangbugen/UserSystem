using UserSystem.Application.Contract.UserApp.Dtos;
using Volo.Abp.Application.Services;

namespace UserSystem.Application.Contract.UserApp
{
    public interface IUserService : IApplicationService
    {
        Task<bool> RegisterUserAsync(UserCreateDto createInput);

        Task<UserDto> GetUserDtoAsync(string userId);

        Task<string> CheckLoginAsync(string userName, string password);

        Task<(string token, bool isSuccess)> RefreshToken();

        Task SaveAsync(string name, Byte[] file);
    }
}