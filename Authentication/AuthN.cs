using ConfidentVotesAPI;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using ConfidentVoteAPI.Authentication;
using Google.Apis.Auth;

namespace AppUserAuthenticaiton{
    public static class AuthEndpoints
    {
        public record GoogleLoginRequest(string IdToken);

        // authentiction 
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/auth");

            // registration
            group.MapPost("/register", async (
                RegisterRequest req,
                UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) =>
            {
                var user = new AppUser
                {
                    UserName = req.UserName,
                    Email = req.Email,
                    FirstName = req.FirstName,
                    LastName = req.LastName
                };
                // create user 
                var result = await userManager.CreateAsync(user, req.Password);
                // if(await roleManager.RoleExistsAsync("User"))
                // {
                //     await roleManager.CreateAsync( new AppRole{ Name = "User"});
                // }
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    return Results.Ok(new { user.Id, user.UserName });
                }

                return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
            });
            //
            group.MapPost("/register/confidentvotes-admin", async (
                RegisterRequest req,
                UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) =>
            {
                var user = new AppUser
                {
                    UserName = req.UserName,
                    Email = req.Email,
                    FirstName = req.FirstName,
                    LastName = req.LastName
                };
                // create user 
                var result = await userManager.CreateAsync(user, req.Password);
                // if(await roleManager.RoleExistsAsync("User"))
                // {
                //     await roleManager.CreateAsync( new AppRole{ Name = "User"});
                // }
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    return Results.Ok(new { user.Id, user.UserName });
                }

                return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
            });

            // login
            group.MapPost("/login", async (
                LoginRequest req,
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IJwtTokensHandler tokensHandler,
                IConfiguration configuration
                ) =>
            {
                var user = await userManager.FindByNameAsync(req.Login);
                
                Console.WriteLine("ok");
                if (user == null)
                    return Results.Unauthorized();
                
                var result = await signInManager.CheckPasswordSignInAsync(
                                        user, req.Password, lockoutOnFailure: false);
                var secretKey = configuration["Jwt:SecretKey"];
                var Issuer = configuration["Jwt:Issuer"];
                var Audience = configuration["Jwt:Audience"];
                if (!result.Succeeded)
                    return Results.Unauthorized();
                var roles = await userManager.GetRolesAsync(user);
                var Accesstoken = tokensHandler.GenerateBearerJwt(user.UserName ?? String.Empty, user.Id.ToString(), roles.ToList(),
                 DateTime.UtcNow.AddMinutes(30),secretKey ?? String.Empty,Issuer ?? String.Empty,Audience ?? String.Empty);
                
                return Results.Ok(new
                {
                    AccessToken = Accesstoken,
                    Roles = roles
                });
            });
            group.MapPost("/google", async (
                GoogleLoginRequest req,
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IJwtTokensHandler tokensHandler,
                IConfiguration configuration
                ) =>
            {
                        GoogleJsonWebSignature.Payload payload;
                try
                {
                    // Verify that the token was genuinely signed by Google for your app
                    var settings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { configuration["Authentication:Google:ClientId"]! }
                    };
                    payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken, settings);
                }
                catch (InvalidJwtException)
                {
                    return Results.Unauthorized();
                }

                // Check if this Google account is already linked to a user
                var user = await userManager.FindByLoginAsync("Google", payload.Subject);

                if (user == null)
                {
                    // Check if a user with this email already registered via password
                    user = await userManager.FindByEmailAsync(payload.Email);

                    if (user == null)
                    {
                        // Create a brand new user WITHOUT a password
                        user = new AppUser
                        {
                            UserName = payload.Email,
                            Email = payload.Email,
                            EmailConfirmed = payload.EmailVerified
                        };

                        var createResult = await userManager.CreateAsync(user);
                        if (!createResult.Succeeded)
                            return Results.BadRequest(createResult.Errors);

                        // Assign default role to new external user
                        await userManager.AddToRoleAsync(user, "User");
                    }

                    // Link the Google provider ID to this user in AspNetUserLogins
                    var loginInfo = new UserLoginInfo("Google", payload.Subject, "Google");
                    await userManager.AddLoginAsync(user, loginInfo);
                }

                var secretKey = configuration["Jwt:SecretKey"];
                var Issuer = configuration["Jwt:Issuer"];
                var Audience = configuration["Jwt:Audience"];
                var roles = await userManager.GetRolesAsync(user);
                var Accesstoken = tokensHandler.GenerateBearerJwt(user.UserName ?? String.Empty, user.Id.ToString(), roles.ToList(),
                 DateTime.UtcNow.AddMinutes(30),secretKey ?? String.Empty,Issuer ?? String.Empty,Audience ?? String.Empty);
                
                return Results.Ok(new
                {
                    AccessToken = Accesstoken,
                    Roles = roles
                });
            });
        }
    }
}