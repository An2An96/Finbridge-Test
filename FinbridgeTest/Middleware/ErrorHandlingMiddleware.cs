using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Finbridge.Test.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Finbridge.Test.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleError(context, ex);
            }
        }

        private static Task HandleError(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            if (ex is ValidationException)
            {
                code = HttpStatusCode.BadRequest;
            }

            var response = JsonSerializer.Serialize(new
            {
                message = ex.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(response);
        }
    }
}