using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MassageStudio.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1) preveri header
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 2) preberi pravi ključ iz appsettings.json
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("ApiKey");

            // 3) primerjaj
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey != potentialApiKey)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // 4) ok -> spusti naprej
            await next();
        }
    }
}
