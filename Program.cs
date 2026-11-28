using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using ConfidentVotesAPI.Services;
using ConfidentVotesAPI;
using ConfidentVotesAPI.CustomApiGenerator;
using Swashbuckle.AspNetCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using ConfidentVotesAPI.CustomExceptionHandling;
using Serilog;
using ConfidentVotesAPI.CustomMiddleWare;
using ConfidentVotesAPI.Utils;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ConfidentVoteAPI.Authentication;
using AppUserAuthenticaiton;

var builder = WebApplication.CreateBuilder(args);
// get the connection string define in user-secret
var connectionString =
    builder.Configuration.GetConnectionString("PsqlDatabase");
// Add services to the container.

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
});
// manages login redirection
builder.Services.ConfigureApplicationCookie( options =>
{
    options.Events.OnRedirectToLogin =  httpContext => {
        if(httpContext.Request.Path.StartsWithSegments("/auth") && 
            httpContext.Response.StatusCode == StatusCodes.Status200OK)
        {
            // make unauthorized response
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
        // default behavior
        httpContext.Response.Redirect(httpContext.RedirectUri);
        return Task.CompletedTask;
    };
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
// db context service
builder.Services.AddDbContext<ConfidentVotesDbContext>();

// identity service
builder.Services.AddIdentityCore<AppUser>(options =>
   {
    options.SignIn.RequireConfirmedEmail = false  ;
   }     
)
.AddRoles<AppRole>()
.AddSignInManager()
.AddEntityFrameworkStores<ConfidentVotesDbContext>()
.AddDefaultTokenProviders();

// authentitication services
// jwt and google
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        RoleClaimType = "role"
    };
}).AddCookie("ExternalCookie")
.AddGoogle(options =>
{
    options.SignInScheme = "ExternalCookie";
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});

// authorization services
builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
                .AddPolicy("Manager", policy => policy.RequireRole("Manager"))
                .AddPolicy("ElectionInitiator", policy => policy.RequireClaim("HasElection","yes"));
// exception services
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
// minimal api configuration
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});
// 
builder.Services.AddScoped<IVotesHandler, VotesHandler>();
//builder.Services.AddScoped<IUsersHandler,UsersHandler>();
builder.Services.AddScoped<IOptionsHandler, OptionsHandler>();
builder.Services.AddScoped<IElectionsHandler, ElectionsHandler>();
builder.Services.AddScoped<IVotersHandler, VotersHandler>();
builder.Services.AddSingleton<IJwtTokensHandler, JwtTokensHandler>();

// logging configuration
Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs/resquestLogs.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
builder.Host.UseSerilog();
var app = builder.Build();
//var rolemanager = app.Services.GetService<RoleManager<AppRole>>();
app.UseExceptionHandler();
// custom middleware
if (!ArgsParser.IsTestWithSwaggerUI(args))
{
  // app.UseMiddleware<GlobalValidator>(); 
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();
app.MapAuthEndpoints();
app.MapPost("/create-role", async (RoleManager<AppRole> roleManager) =>
{
    if( !await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new AppRole{ Name = "Admin"});
    }
    if( !await roleManager.RoleExistsAsync("Manager"))
    {
        await roleManager.CreateAsync(new AppRole{ Name = "Manager"});
    }
    if( !await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new AppRole{ Name = "User"});
    }
});
app.MapGet("/", () => @"/confidentvotesapi is the base path
/ and /votes are repsectively the endpoints to retrieve users and votes data!");
// catch-all parameters : / is a delimiter if just used a simple string 
// we get one string captured
/*
app.MapGet("/api/{*filePath}", (string filePath) => filePath);
// query parameters it get thru query
app.MapGet("/api/search", (string? query, int page = 2) => @$"{query} and {page}");
*/
await app.StartAsync();
// generate API client
if (ArgsParser.IsGenerateapiclient(args))
{
    await  new ClientGenerator("http://localhost",5284).GenerateClient();
}
 

await app.WaitForShutdownAsync();
//app.Run();



