using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UserSystem.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace UserSystem.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule),
        typeof(UserSystemDomainModule)
    )]
    public class UserSystemEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<UserSystemDbContext>(opts =>
            {
                opts.AddDefaultRepositories(true);
            });

            context.Services.AddScoped<IWorkOfUnit, WorkOfUnit>();

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseMySQL();
            });

            base.ConfigureServices(context);
        }
    }
}