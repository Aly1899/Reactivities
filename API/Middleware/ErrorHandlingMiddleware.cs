using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Application;
using Application.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
      this._logger = logger;
      this._next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try 
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HanldeExeptionAsync(context, ex, _logger); 
        }
    }

    private async Task HanldeExeptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
    {
      object errors = null;

      switch (ex)
      {
          case RestException re:
                logger.LogError(ex, "REST ERROR");
                errors = re.Errors;
                context.Response.StatusCode = (int)re.Code;
                break;
            case Exception e:
                logger.LogError(ex, "SERVER ERROR");
                errors = string.IsNullOrWhiteSpace(e.Message) ? "ERROR" : e.Message;
                context.Response.StatusCode =(int)HttpStatusCode.InternalServerError;
                break;
      }

      context.Response.ContentType = "application/json";
      if (errors != null)
      {
          var result = JsonSerializer.Serialize(new 
          {
              errors
          });

          await context.Response.WriteAsync(result);
      }
        
    }
  }
}