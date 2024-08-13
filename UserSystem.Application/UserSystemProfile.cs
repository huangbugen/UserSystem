using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserSystem.Application.Contract.UserApp.Dtos;
using UserSystem.Domain.Account;

namespace UserSystem.Application
{
    public class UserSystemProfile : Profile
    {
        public UserSystemProfile()
        {
            // CreateMap<UserCreateDto, User>();
            CreateMap<UserCreateDto, User>().ConstructUsing((dto, ctx) => new User(Guid.NewGuid().ToString("N")));
            CreateMap<User, UserDto>();
            CreateMap<UserLevel, UserLevelDto>();
        }
    }
}