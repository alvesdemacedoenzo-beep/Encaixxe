using Encaixxe.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Encaixxe.Web.Controllers;

public class ContaController : Controller
{
    private readonly IConfiguration _configuration;

    public ContaController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var adminEmail = _configuration["AdminUser:Email"];
        var adminSenha = _configuration["AdminUser:Senha"];

        if (model.Email != adminEmail || model.Senha != adminSenha)
        {
            ModelState.AddModelError("", "E-mail ou senha inválidos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Email),
            new Claim(ClaimTypes.Email, model.Email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}