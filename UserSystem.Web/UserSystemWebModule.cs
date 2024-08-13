using Microsoft.OpenApi.Models;
using Qubiancheng.Abp.AspNet.JwtBearer;
using UserSystem.Application;
using UserSystem.Web.Filters;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Modularity;

namespace UserSystem.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(UserSystemApplicationModule),
        typeof(QubianchengAbpAspNetJwtBearerModule),
        typeof(AbpAutofacModule),
        typeof(AbpBlobStoringMinioModule),
        typeof(AbpAspNetCoreSignalRModule)
    )]
    public class UserSystemWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddControllers();
            var configuration = context.Services.GetConfiguration();
            var origins = configuration.GetSection("AllowOrigins").Get<string[]>();
            context.Services.AddCors(c => c.AddDefaultPolicy(policy => policy.AllowAnyMethod().AllowAnyHeader().WithOrigins(origins!).AllowCredentials()));
            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserSystem", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "xxxxxxxxxxxxxx",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                          Reference = new OpenApiReference{
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                          }
                      },
                      new string[]{}
                    }

                });
                c.DocInclusionPredicate((docName, description) => true);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();
        }
    }
}