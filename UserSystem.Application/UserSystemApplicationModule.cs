using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystem.EntityFrameworkCore;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace UserSystem.Application
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(UserSystemEntityFrameworkCoreModule)
    )]
    public class UserSystemApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<UserSystemProfile>();
            });
            base.ConfigureServices(context);
        }
    }
}