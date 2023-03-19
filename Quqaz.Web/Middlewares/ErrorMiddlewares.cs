using Quqaz.Web.CustomException;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quqaz.Web.Middlewares
{
    public class ErrorMiddlewares
    {
        private readonly RequestDelegate _next;
        private Logging _logging;
        public ErrorMiddlewares(RequestDelegate next)
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

                var response = context.Response;
                response.ContentType = "application/json";
                dynamic responseBody;
                switch (ex)
                {
                    case ConflictException cex:
                        {
                            response.StatusCode = StatusCodes.Status409Conflict;
                            responseBody = cex.Errors;
                        }
                        break;
                    default:
                        {
                            _logging = context.RequestServices.GetService(typeof(Logging)) as Logging;
                            _logging.WriteExption(ex);
                            var uintOfWork = context.RequestServices.GetService(typeof(IUintOfWork)) as IUintOfWork;
                            if (uintOfWork != null && uintOfWork.IsTransactionOpen)
                            {
                                await uintOfWork.Rollback();
                            }
                            response.StatusCode = StatusCodes.Status400BadRequest;
                            responseBody = ex.Message;
                        }
                        break;

                }
                string result = JsonSerializer.Serialize(responseBody, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                await response.WriteAsync(result);

            }

        }
    }
}
