using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            this.userManager = userManager;

            this.signInManager = signInManager;

            this.logger = logger;

        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Book");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("List", "Book");
                }
                
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Site");
        }
    }
}