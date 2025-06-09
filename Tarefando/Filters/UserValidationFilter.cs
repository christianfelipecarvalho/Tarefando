using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tarefando.Infrastructure.Data;

namespace Tarefando.Filters
{
    public class UserValidationFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var dbContext = httpContext.RequestServices.GetRequiredService<TarefandoDbContext>();

            if (httpContext.Request.Headers.TryGetValue("User-Id", out var userIdValue) &&
                int.TryParse(userIdValue, out var userId))
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    context.Result = new BadRequestObjectResult(new { error = "Usuário não encontrado" });
                    return;
                }

                httpContext.Items["UserId"] = userId;
                httpContext.Items["User"] = user;
            }

            await next();
        }
    }
}