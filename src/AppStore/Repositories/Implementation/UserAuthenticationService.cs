using Microsoft.AspNetCore.Identity;
using AppStore.Models.DTO;
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
using System.Runtime.CompilerServices;

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
            status.StatusCode = 0;
            status.Message = "El usuario es invalido";
            return status;
        }

        if (!await _userManager.CheckPasswordAsync(user, login.Password!))
        {
            status.StatusCode = 0;
            status.Message = "El password es incorrecto";
            return status;
        }

        var resultado = await _signInManager.PasswordSignInAsync(user, login.Password!, true, false);
        if (!resultado.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "Las credenciales son incorrectas";
        }
        else
        {
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
