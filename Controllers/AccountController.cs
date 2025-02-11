using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Threading.Tasks;

namespace ShoppingCartApp.Controllers;

public class AccountController : Controller
{
    public IActionResult Login(string returnUrl = "/")
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse", new { returnUrl }) };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return RedirectToAction("Login");

        // Aquí podrías extraer información del usuario y crear un perfil local si lo deseas

        return LocalRedirect(returnUrl);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
