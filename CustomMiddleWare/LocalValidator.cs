using ConfidentVotesAPI.CustomExceptionHandling;
namespace ConfidentVotesAPI.CustomMiddleWare
{
    public sealed class LocalValidator
    {
        public readonly RequestDelegate _next;
        private const string CONFIDENT_API_ID = "CONFIDENT_API_kaf@#fam";
        public LocalValidator(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        { 
           await  _next(context);
            
        }
    }
}