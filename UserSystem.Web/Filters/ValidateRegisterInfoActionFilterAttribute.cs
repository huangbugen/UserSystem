using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserSystem.Application.Contract.UserApp.Dtos;
using UserSystem.Domain.Account;
using Volo.Abp.Domain.Repositories;

namespace UserSystem.Web.Filters
{
    public class ValidateRegisterInfoActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly IRepository<User> _userRepo;

        public ValidateRegisterInfoActionFilterAttribute(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var ip = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            // 通过第三方的ip地址包进行匹配
            // .......

            var input = context.ActionArguments["createInput"] as UserCreateDto;

            // 验证邮箱的准确性
            Regex regexEmail = new Regex("^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");
            if (regexEmail.IsMatch(input!.Email))
            {
                if (_userRepo.AnyAsync(m => m.Email == input.Email).Result)
                {
                    context.Result = new JsonResult(null)
                    {
                        Value = "该邮箱已注册",
                        StatusCode = 202
                    };
                }
            }
            else
            {
                context.Result = new JsonResult(null)
                {
                    Value = "邮箱格式不正确",
                    StatusCode = 202
                };
            }

            if (input.UserName.Length < 2 || input.UserName.Length > 10)
            {
                context.Result = new JsonResult(null)
                {
                    Value = "用户名长度不符合规范",
                    StatusCode = 202
                };
            }
            else if (_userRepo.AnyAsync(m => m.UserName == input.UserName).Result)
            {
                context.Result = new JsonResult(null)
                {
                    Value = "用户名已注册",
                    StatusCode = 202
                };
            }

            Regex regexPhone = new Regex(@"^(?:(?:\+|00)86)?1[3-9]\d{9}$");
            if (regexPhone.IsMatch(input.UserNo))
            {
                if (_userRepo.AnyAsync(m => m.UserNo == input.UserNo).Result)
                {
                    context.Result = new JsonResult(null)
                    {
                        Value = "手机号已注册",
                        StatusCode = 202
                    };
                }
            }
            else
            {
                context.Result = new JsonResult(null)
                {
                    Value = "手机号格式不正确",
                    StatusCode = 202
                };
            }
        }
    }
}