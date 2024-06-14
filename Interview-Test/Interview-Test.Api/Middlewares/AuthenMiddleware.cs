using System.Security.Cryptography;
using System.Text;

namespace Interview_Test.Middlewares;

public class AuthenMiddleware : IMiddleware
{
    private const string hashedKey = "<your hash sha512 x-api-key>";
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var apiKeyHeader = context.Request.Headers["x-api-key"];
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            context.Response.StatusCode = 401;
            return context.Response.WriteAsync("API Key is missing");
        }
        
        // Implement validate x-api-key to authenticate the user here
        //throw new NotImplementedException();
        
        try
        {
            SHA512 mySHA512 = SHA512.Create();
            byte[] byteArray = Encoding.UTF8.GetBytes(apiKeyHeader);
            byte[] hashedBytes = mySHA512.ComputeHash(byteArray);
            string result = Encoding.UTF8.GetString((hashedBytes));
            if (hashedKey != result)
            {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Key is Invalid");
            }

            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("Key is Valid");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}