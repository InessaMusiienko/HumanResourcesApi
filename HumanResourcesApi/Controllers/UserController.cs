using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static HumanResources.Areas.Identity.Pages.Account.LoginModel;
using HumanResourcesApi.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HumanResourcesApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;

        public UserController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }



        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null) //OnGetAsync
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Home", "Index");

            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
            return new PageResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //if (this.User.IsInRole("admin"))
                //{
                //    return this.RedirectToAction("/admin/adminhome/transactionsreview");
                //}

                return this.Redirect("/home/index");
            }

            if (this.ModelState.IsValid)
            {
                bool rememberMe = model.RememberMe == "on" ? true : false;

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, rememberMe, false);

                if (result.Succeeded)
                {
                    if (this.User.IsInRole("admin"))
                    {
                        return this.Redirect("/admin/adminhome/transactionsreview");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return this.RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //if (this.User.IsInRole("admin"))
                //{
                //    return this.Redirect("/admin/adminhome/transactionsreview");
                //}

                return this.Redirect("/home/index");
            }

            ReturnUrl = returnUrl;
            return new PageResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //if (this.User.IsInRole("admin"))
                //{
                //    return this.Redirect("/admin/adminhome/transactionsreview");
                //}

                return this.Redirect("/home/index");
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Login", "User");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return this.RedirectToAction("Register");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "User");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}