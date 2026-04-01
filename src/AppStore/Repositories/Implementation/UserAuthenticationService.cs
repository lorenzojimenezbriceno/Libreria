using Microsoft.AspNetCore.Identity;
using AppStore.Models.DTO;
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Abstract;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    public UserAuthenticationService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Status> LoginAsync(LoginModel login)
    {
        var status = new Status();
        var user = await _userManager.FindByNameAsync(login.Username!);

        if (user == null)
        {
            status.IsSuccessful = false;
            status.StatusCode = 0;
            status.Message = "El usuario es invalido";
            return status;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password!);

        if (!result.Succeeded)
        {
            status.IsSuccessful = false;
            status.StatusCode = 0;
            status.Message = "El password es incorrecto";
            return status;
        }

        var resultado = await _signInManager.PasswordSignInAsync(user, login.Password!, true, false);
        if (!resultado.Succeeded)
        {
            status.IsSuccessful = false;
            status.StatusCode = 0;
            status.Message = "Las credenciales son incorrectas";
        }
        else
        {
            status.IsSuccessful = true;
            status.StatusCode = 1;
            status.Message = "Login exitoso";
        }

        return status;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
