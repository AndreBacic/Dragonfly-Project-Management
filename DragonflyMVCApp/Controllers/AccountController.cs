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

            return RedirectToAction(nameof(Home));
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login), "Account"); // todo: Make this an actual page, or just delete the associated view?
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
                return View();
            }
            UserModel newDbUser = new()
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                EmailAddress = newUser.EmailAddress,
                PasswordHash = HashAndSalter.HashAndSalt(newUser.Password).ToDbString()
            };
            _db.CreateUser(newDbUser);

            LogInUser(newDbUser);

            return Home();
        }

        [Authorize]
        public IActionResult EditAccount()
        {
            UserModel user = this.GetLoggedInUserByEmail(_db);

            return View(user.DbUserToEditView());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditUserViewModel updatedUser)
        {
            //bool emailTaken = _db.GetUser(updatedUser.EmailAddress) is null;
            UserModel loggedInUser = this.GetLoggedInUserByEmail(_db);

            // TODO: Finish this edit account logic

            return View();
        }

        /// <summary>
        /// The Home page for the user, and where the user may select an assignment 
        /// to work on, or to search for organizations or other users.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult MyProjects()
        {
            LogInUser(this.GetLoggedInUserByEmail(_db));
            return View();
        }

        private async void LogInUser(UserModel user)
        {
            List<Claim> personClaims = new()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, UserRoles.USER) // demo user has "Demo user" role
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(personClaims, "Dragonfly.Auth.Identity")));
        }

        private static bool IsValidEmailAddress(string emailAddress) // todo: move to an email logic class
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
    }
}