using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using authenticationMvc.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace authenticationMvc.Controllers
{
    public class AccountController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly TestUserStore _testUserStore;

        public AccountController(TestUserStore testUserStore)
        {
            _testUserStore = testUserStore;
        }

        //public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ViewData["ReturnUrl"] = returnUrl;
            //var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            //if (user == null)
            //{

            //}

            //await _signInManager.SignInAsync(user, new AuthenticationProperties { IsPersistent = true });
            var user = _testUserStore.FindByUsername(loginViewModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError(nameof(loginViewModel.UserName), "UserName not exists");
            }
            else
            {
                if (_testUserStore.ValidateCredentials(loginViewModel.UserName, loginViewModel.Password))
                {
                    var props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                    };
                    await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(HttpContext,
                        user.SubjectId, user.Username, props);
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError(nameof(loginViewModel.Password), "Wrong Password");
            }
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            //ViewData["ReturnUrl"] = returnUrl;
            //var identityUser = new ApplicationUser
            //{
            //    Email = registerViewModel.Email,
            //    UserName = registerViewModel.Email,
            //    NormalizedUserName = registerViewModel.Email
            //};

            //var identityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);

            //if (identityResult.Succeeded)
            //{
            //    await _signInManager.SignInAsync(identityUser, new AuthenticationProperties { IsPersistent = true });
            //    return RedirectToLocal(returnUrl);
            //}
            //else
            //{
            //    foreach (var error in identityResult.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}

            return View();
        }

        public IActionResult MakeLogin()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "lucy"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
