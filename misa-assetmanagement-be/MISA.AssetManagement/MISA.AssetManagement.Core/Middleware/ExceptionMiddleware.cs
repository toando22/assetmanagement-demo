using Microsoft.AspNetCore.Http;
using MISA.AssetManagement.Core.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Middleware
{
        /// <summary>
        /// Middleware xử lý lỗi toàn cục
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:
        /// </summary>
        public class ExceptionMiddleware
        {
            private readonly RequestDelegate _next;

            public ExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (System.Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
            {
                context.Response.ContentType = "application/json";

                var responseError = new MISAError
                {
                    TraceId = context.TraceIdentifier
                };

                if (exception is MISAValidateException validateException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    responseError.ErrorCode = "400";
                    responseError.UserMsg = validateException.ValidateErrorMsg ?? "Dữ liệu không hợp lệ.";
                    responseError.DevMsg = validateException.Message;
                    // QUAN TRỌNG: Gán Dictionary lỗi vào response
                    responseError.Errors = validateException.Errors;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    responseError.ErrorCode = "500";
                    responseError.UserMsg = "Có lỗi xảy ra, vui lòng liên hệ MISA.";
                    responseError.DevMsg = exception.Message;
                }

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DictionaryKeyPolicy = null
                };

                var jsonResponse = JsonSerializer.Serialize(responseError, jsonOptions);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    
}
