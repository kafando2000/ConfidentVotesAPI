using ConfidentVotesAPI;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AppUserAuthenticaiton;
public static class AuthEndpoints
{
    // authentiction 
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/confidentvotesapi/auth");

        // registration
        group.MapPost("/register", async (
            RegisterRequest req,
            UserManager<AppUser> userManager) =>
        {
            var user = new AppUser
            {
                UserName = req.Email,
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName
            };

            var result = await userManager.CreateAsync(user, req.Password);

            return result.Succeeded
                ? Results.Ok(new { user.Id, user.Email })
                : Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
        });

        // login
        group.MapPost("/login", async (
            LoginRequest req,
            SignInManager<AppUser> signInManager) =>
        {
            // Uses Identity cookies or bearer tokens depending on your configuration
            var result = await signInManager.PasswordSignInAsync(
                req.Login, req.Password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded
                ? Results.Ok()
                : Results.Unauthorized();
        });
    }
}
