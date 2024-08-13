using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserSystem.EntityFrameworkCore
{
    public class UserSystemDbContextFactory : IDesignTimeDbContextFactory<UserSystemDbContext>
    {
        public UserSystemDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<UserSystemDbContext>()
            .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);
            return new UserSystemDbContext(builder.Options);
        }
        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../UserSystem.Web/"))
            .AddJsonFile("appsettings.json", false);
            return builder.Build();
        }
    }
}