using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using UserSystem.EntityFrameworkCore;

namespace UserSystem.Web.Filters
{
    public class WorkOfUnitActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var workOfUnit = context.HttpContext.RequestServices.GetRequiredService<IWorkOfUnit>();

            workOfUnit.BeginTransaction();

            var res = await next();

            if (res.Exception == null || res.ExceptionHandled)
            {
                workOfUnit.Commit();
            }
        }
    }
}