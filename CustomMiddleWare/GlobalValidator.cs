using ConfidentVotesAPI.CustomExceptionHandling;
using Microsoft.AspNetCore.Builder.Extensions;
namespace ConfidentVotesAPI.CustomMiddleWare
{
    public sealed class GlobalValidator 
    {
        public readonly RequestDelegate _next;
        // api signature
        private const string CONFIDENT_API_ID = "CONFIDENT_API_kaf@#fam";
        public GlobalValidator(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {   
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }
           if(!context.Request.Headers.ContainsKey("API-SIGNATURE") || 
                context.Request.Headers["API-SIGNATURE"] != CONFIDENT_API_ID)
            { 
                throw new BadHeaderException("API-SIGNATURE");
                
            }
           await  _next(context);
            
        }
    }
}