using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next ;
        public readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this._env = env;
            this._logger = logger;
            this._next = next;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType="application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

                var response=_env.IsDevelopment()?
                    new ApiException((int)HttpStatusCode.InternalServerError,ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);

                    var option=new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                    var json=JsonSerializer.Serialize(response,option);
                    await context.Response.WriteAsync(json);
            }
        }
    }
}