using Microsoft.AspNetCore.Mvc;
using UserSystem.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();

builder.Services.AddApplication<UserSystemWebModule>();

var app = builder.Build();

app.InitializeApplication();

app.MapControllers();

app.MapGet("/heart", () =>
            {
                return new OkResult();
            });

app.Run();