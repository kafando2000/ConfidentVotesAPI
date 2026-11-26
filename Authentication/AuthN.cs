using ConfidentVotesAPI;
using ConfidentVotesAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AppUserAuthenticaiton;
public static class AuthEndpoints
{
    // authentiction 
    public static void MapAuthEndpoints(this RouteGroupBuilder app)
    {
        var group = app.MapGroup("/confidentvotesapi/auth");

        // registration
        group.MapPost("/register", async (
            RegisterRequest req,
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) =>
        {
            var user = new AppUser
            {
                UserName = req.Email,
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
