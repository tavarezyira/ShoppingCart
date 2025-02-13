using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using ShoppingCartApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingCartApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            return RedirectToAction("CustomerLogin", new { returnUrl });
        }

        public IActionResult GoogleLogin(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse", new { returnUrl }) };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return RedirectToAction("CustomerLogin");

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return RedirectToAction("CustomerLogin");
        }

        var existingUser = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        if (existingUser == null)
        {
            var newUser = new Customer { Email = email, Password = HashPassword("GoogleAuthUser") };
            _context.Customers.Add(newUser);
            await _context.SaveChangesAsync();
        }

        // Sign in the user
        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim("UserType", "Customer")
        };

        var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return LocalRedirect(returnUrl);
    }


        // --- Admin Login ---
        [HttpGet]
        public IActionResult AdminLogin(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model, string returnUrl = "/")
        {
            if (!ModelState.IsValid) return View(model);

            string storedAdminEmail = "tavarezyira@gmail.com";
            string storedAdminPasswordHash = HashPassword("AdminContra"); // Hash your password

            if (model.Email == storedAdminEmail && HashPassword(model.Password) == storedAdminPasswordHash)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid admin credentials");
            return View(model);
        }

        // --- Customer Registration ---
        [HttpGet]
        public IActionResult CustomerRegister(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerRegister(CustomerRegisterViewModel model, string returnUrl = "/")
        {
            if (!ModelState.IsValid) return View(model);

            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("Email", "There's already an account with this email address.");
                return View(model);
            }

            var customer = new Customer
            {
                Email = model.Email,
                Password = HashPassword(model.Password) // Secure password storage
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Email),
                new Claim("UserType", "Customer")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return LocalRedirect(returnUrl);
        }

        // --- Customer Login ---
        [HttpGet]
        public IActionResult CustomerLogin(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerLogin(CustomerLoginViewModel model, string returnUrl = "/")
        {
            if (!ModelState.IsValid) return View(model);

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer == null || !VerifyPassword(model.Password, customer.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Email),
                new Claim("UserType", "Customer")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return LocalRedirect(returnUrl);
        }

        // --- Logout ---
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // --- Password Hashing Helpers ---
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return HashPassword(inputPassword) == storedHash;
        }
    }
}
