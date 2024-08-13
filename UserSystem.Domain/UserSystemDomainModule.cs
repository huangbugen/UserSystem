using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Qubiancheng.Abp.AspNet.JwtBearer;
using Qubiancheng.Abp.Microservice.Consul;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace UserSystem.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(QubianchengAbpMicroserviceConsulModule)
    )]
    public class UserSystemDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // context.Services.AddSingleton<ILoggerFactory, NLogLoggerFactory>();
            context.Services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.AddNLog("Config/NLog.config");
            });

            Configure<RedisCacheOptions>(options =>
            {
                // ...
            });

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseMinio(minio =>
                    {
                        minio.EndPoint = "127.0.0.1:9000";
                        minio.AccessKey = "znBnfBRr3QIGOfrurqPO";
                        minio.SecretKey = "oNR8VRKlNXtCx72F2j50PYqP75geDAxs545qd9pw";
                        minio.BucketName = "test";
                    });
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // base.OnApplicationInitialization(context);
        }
    }
}