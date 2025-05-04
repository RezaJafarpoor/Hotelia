using Hotelia.Shared.Auth.Dtos;
using Hotelia.Shared.Common;
using Hotelia.Shared.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hotelia.Shared.Auth;

public class AuthService
    (UserManager<User> userManager,
        TokenGenerator tokenGenerator,
        SignInManager<User> signInManager)
{


    public async Task<Result<AuthResponse>> CreateAccount(CreateUserDto dto)
    {
        var user = new User { Email = dto.Email };
        
        var createdUser = await userManager.CreateAsync(user);
        if (!createdUser.Succeeded)
        {
            var errors = createdUser.Errors.Select(e => e.Description).ToList();
            return Result<AuthResponse>.Failed(errors);
        }

        var result = await userManager.AddPasswordAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<AuthResponse>.Failed(errors);
        }

        var accessToken = GetToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(accessToken));
    }


    public async Task<Result<AuthResponse>> Login(LoginDto dto)
    {
        var userExist = await userManager.FindByEmailAsync(dto.Email);
        if (userExist is null)
            return Result<AuthResponse>.Failed("user or password is wrong");
        var loginResult = await signInManager.CheckPasswordSignInAsync(userExist, dto.Password, false);
        if (!loginResult.Succeeded)
            return Result<AuthResponse>.Failed("user or password is wrong");

        var accessToken = GetToken(userExist);
        return Result<AuthResponse>.Success(new AuthResponse(accessToken));
    }
    
    

    private string GetToken(User user) 
        => tokenGenerator.GenerateToken(user);
    
}