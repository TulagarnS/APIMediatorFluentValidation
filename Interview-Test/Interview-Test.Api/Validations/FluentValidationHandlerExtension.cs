
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Utility.Package.Extensions;

namespace Interview_Test.Validations;

public static class FluentValidationHandlerExtension
{
    public static void UseFluentValidationHandlerExtension(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(x =>
        {
            x.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature?.Error;

                if (!(exception is ValidationException validationException))
                {
                    throw exception!;
                }

                var error = validationException.Errors.Select(err => new
                {
                    err.ErrorCode,
                    err.ErrorMessage,
                    err.AttemptedValue
                }).FirstOrDefault();

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                int.TryParse(error?.ErrorCode ?? "200", out var _out);
                try
                {
                    if (error?.AttemptedValue is Dictionary<string, string> attemptedValues)
                    {
                        var responseValidate = new BaseResponseExtension
                        {
                            StatusCode = (_out == 0?400:_out),
                            StatusDesc = error.ErrorMessage
                        };
                        foreach (var err in attemptedValues)
                        {
                            responseValidate.StatusDesc = err.Value + responseValidate.StatusDesc;
                        }

                        await context.Response.WriteAsync(responseValidate.SerializeCamelCaseCore());
                    }
                    else
                    {
                        await context.Response.WriteAsync(new BaseResponseExtension
                        {
                            StatusCode = (_out == 0?400:_out),
                            StatusDesc = error!.ErrorMessage + error!.AttemptedValue
                        }.SerializeCamelCaseCore());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        });
    }
}

public class BaseResponseExtension
{
    public int StatusCode { get; set; }
    public string StatusDesc { get; set; }
}