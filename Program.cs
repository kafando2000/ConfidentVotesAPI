using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using ConfidentVotesAPI.Services;
using Swashbuckle.AspNetCore;
using Microsoft.AspNetCore.OpenApi;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using ConfidentVotesAPI.CustomMiddleWare;
using ConfidentVotesAPI.utils;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
// excetion services
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// api controller services 
builder.Services.AddControllers()
                .AddJsonOptions(options => {
                            options.JsonSerializerOptions.UnmappedMemberHandling = 
                            System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
                            options.JsonSerializerOptions.AllowTrailingCommas = true;
                            options.JsonSerializerOptions.PropertyNamingPolicy = 
                            JsonNamingPolicy.SnakeCaseLower;
                });
builder.Services.AddSingleton<IVotesHandler,VotesHandler>();
builder.Services.AddSingleton<IUsersHandler,UsersHandler>();
// logging configuration
Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs/resquestLogs.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
builder.Host.UseSerilog();
var app = builder.Build();

app.UseExceptionHandler();
// custom middleware
if (!ArgsParser.IsTestWithSwaggerUI(args))
{
   app.UseMiddleware<GlobalValidator>(); 
}
// simple middleware implementation
app.Use( async (context, next) =>
{   
    Console.WriteLine(context.Request.Path);
    await next.Invoke();
    Console.WriteLine(context.Response.StatusCode);
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();


app.MapGet("/", () => @"/api is the base url  
            and /users is the endpoint to retrieve users data!");
app.MapPut("/", () => @"/api is the base url  
            and /users is the endpoint to add users data!");
// catch-all parameters : / is a delimiter if just used a simple string 
// we get one string captured
/*
app.MapGet("/api/{*filePath}", (string filePath) => filePath);
// query parameters it get thru query
app.MapGet("/api/search", (string? query, int page = 2) => @$"{query} and {page}");
*/


// generated API client usage
// var httpClient = new HttpClient();
// var client = new CustomApiClient("http://localhost:5000", httpClient);

// var user = await client.UserAsync(1);
// Console.WriteLine($"Fetched User: {user}");

app.Run();



