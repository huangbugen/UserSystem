using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using NUglify.Helpers;
using UserSystem.Domain.Shared.SharedModels;
using Volo.Abp.Caching;

namespace UserSystem.Web.Filters
{
    public class UserInfoAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly IDistributedCache<UserRefreshToken> _refreshCache;

        public UserInfoAuthorizationFilter(IDistributedCache<UserRefreshToken> refreshCache)
        {
            _refreshCache = refreshCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var refreshToken = context.HttpContext.Request.Cookies["refreshToken"];
            UserRefreshToken userRefreshToken = _refreshCache.GetAsync(refreshToken!).Result!;
            var user = userRefreshToken.User;
            var props = user.GetType().GetProperties();
            foreach (var prop in props)
            {
                var key = prop.Name;
                var value = prop.GetValue(user);
                context.HttpContext.Items.Add(key, value);
            }
        }
    }
}