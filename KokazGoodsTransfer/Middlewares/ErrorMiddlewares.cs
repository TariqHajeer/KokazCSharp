using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Middlewares
{
    public class ErrorMiddlewares
    {
        private readonly RequestDelegate _next;
        private  Logging _logging;
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
                _logging = context.RequestServices.GetService(typeof(Logging)) as Logging;
                _logging.WriteExption(ex);
                var response = context.Response;
                response.ContentType = "application/json";
                dynamic responseBody;
                switch (ex)
                {
                    case ConfilectException cex:
                        {
                            response.StatusCode = StatusCodes.Status409Conflict;
                            responseBody = cex.Errors;
                        }
                        break;
                    default:
                        {
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
