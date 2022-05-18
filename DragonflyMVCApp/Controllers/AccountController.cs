using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;
using DragonflyMVCApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace DragonflyMVCApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDataAccessor _db;

        public AccountController(IDataAccessor db)
        {
            _db = db;
        }
        
        // Landing page
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel user)
        {
            UserModel dbUser;
            try
            {
                dbUser = _db.GetUser(user.EmailAddress);
                if (dbUser is null)
                {
                    return View(); // TODO: Add bad login message to all of these 'return View();'s
                }
            }
            catch
            {
                return View();
            }

            PasswordHashModel passwordHash = new();
            passwordHash.FromDbString(dbUser.PasswordHash);

            (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(user.Password, passwordHash);

            if (IsPasswordCorrect == false)
            {
                return View();
            }

            LogInUser(dbUser);

            return RedirectToAction(nameof(HomeController.Home), "Home");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Register()
        {
            ViewData["RegisterMessage"] = "";
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel newUser)
        {
            if (ModelState.IsValid == false)
            {
                ViewData["RegisterMessage"] = "Invalid Inputs";
                return View(newUser);
            }

            if (_db.GetUser(newUser.EmailAddress) is not null)
            {
                ViewData["RegisterMessage"] = "That email address is taken";
                return View(newUser);
            }
            UserModel newDbUser = new()
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                EmailAddress = newUser.EmailAddress,
                PasswordHash = HashAndSalter.HashAndSalt(newUser.Password).ToDbString(),
                // dark mode is default because it causes less eye strain
                ColorPrefference = ColorPrefference.Dark 
            };
            _db.CreateUser(newDbUser);

            LogInUser(newDbUser);

            return RedirectToAction(nameof(HomeController.Home), "Home");
        }

        //redirect action that first logs user in as demo user
        public IActionResult DemoLogin()
        {
            UserModel demoUser = new();// = exampleUserSingleton ?
            LogInUser(demoUser, UserRoles.DEMO_USER);
            return RedirectToAction(nameof(HomeController.Home), "Home");
        }

        private async void LogInUser(UserModel user, string role = UserRoles.USER)
        {
            List<Claim> personClaims = new()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, role) // demo user has "Demo user" role
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(personClaims, "Dragonfly.Auth.Identity")));
        }

        private static bool IsValidEmailAddress(string emailAddress) // TODO: move to an email logic class
        {
            try
            {
                System.Net.Mail.MailAddress m = new(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// This just handles the error message page
        /// </summary>
        /// <returns>A web page displaying an error message</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}