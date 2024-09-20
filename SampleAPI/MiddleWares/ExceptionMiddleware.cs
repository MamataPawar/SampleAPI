using Microsoft.OpenApi.Models;
using SampleAPI.Entities;
using System.Net;

namespace SampleAPI.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                ApiError apiErrorResponse;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                string message = "Some unknown error occured"; ;

                if (environment.IsDevelopment())
                {
                    apiErrorResponse = new ApiError((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else
                {
                    apiErrorResponse = new ApiError((int)statusCode, message);
                }

                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(apiErrorResponse.ToString());
            }
        }
    }
}
