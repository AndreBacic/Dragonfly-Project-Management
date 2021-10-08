using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Bug_Tracker_Library.DataAccess;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
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
            try
            {
                UserModel dbUser = _db.GetUser(user.EmailAddress);

                PasswordHashModel passwordHash = new();
                passwordHash.FromDbString(dbUser.PasswordHash);

                (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(user.Password, passwordHash);

                if (IsPasswordCorrect)
                {
                    LogInUser(dbUser);

                    return RedirectToAction(nameof(OrganizationController.OrganizationHome), "Organization");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        [Authorize("Logged_in_user_policy")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login), "Account"); // todo: Make this an actual page, or just delete the associated view?
        }
        public IActionResult LoginFailed()
        {
            return View();
        }
        public IActionResult Register()
        {
            ViewData["RegisterMessage"] = "";
            return View();
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

            List<UserModel> allUsers = _db.GetAllUsers();
            if (allUsers.Any(x => x.EmailAddress == newUser.EmailAddress))
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

            return RedirectToAction(nameof(OrganizationController.OrganizationHome), "Organization");
        }

        [Authorize("Logged_in_user_policy")]
        public IActionResult EditAccount()
        {
            UserModel user = GetLoggedInUserByEmail();

            return View(user.DbUserToEditView());
        }
        [Authorize("Logged_in_user_policy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditUserViewModel updatedUser)
        {
            // 1) Make sure email isn't taken
            List<UserModel> allUsers = _db.GetAllUsers();
            UserModel loggedInUser = GetLoggedInUserByEmail();

            if (IsValidEmailAddress(updatedUser.EmailAddress) == false ||
                allUsers.Any(x => x.EmailAddress == updatedUser.EmailAddress && updatedUser.EmailAddress != loggedInUser.EmailAddress))
            {
                ViewData["EditMessage"] = "That email address is taken"; // todo: refactor this viewdata message system
                return View(loggedInUser.DbUserToEditView());
            }

            if (string.IsNullOrWhiteSpace(updatedUser.NewPassword) == false)
            {
                // 2) Make sure old password is correct
                PasswordHashModel passwordHash = new PasswordHashModel();
                passwordHash.FromDbString(loggedInUser.PasswordHash);

                (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(updatedUser.OldPassword, passwordHash);

                if (IsPasswordCorrect)
                {
                    loggedInUser.FirstName = updatedUser.FirstName;
                    loggedInUser.LastName = updatedUser.LastName;
                    loggedInUser.EmailAddress = updatedUser.EmailAddress;
                    loggedInUser.PasswordHash = HashAndSalter.HashAndSalt(updatedUser.NewPassword).ToDbString();
                    _db.UpdateUser(loggedInUser);

                    LogInUser(loggedInUser);

                    loggedInUser.EmailAddress = "";
                    loggedInUser.PasswordHash = "";
                    return RedirectToAction(nameof(OrganizationController.OrganizationHome), "Organization");
                }
                else
                {
                    return View(loggedInUser.DbUserToEditView());
                }
            }
            else
            {
                // No password change
                loggedInUser.FirstName = updatedUser.FirstName;
                loggedInUser.LastName = updatedUser.LastName;
                loggedInUser.EmailAddress = updatedUser.EmailAddress;
                _db.UpdateUser(loggedInUser);

                LogInUser(loggedInUser);

                return RedirectToAction(nameof(OrganizationController.OrganizationHome), "Organization");
            }
        }

        /// <summary>
        /// The Home page for the user, and where the user may select an assignment 
        /// to work on, or to search for organizations or other users.
        /// </summary>
        /// <returns></returns>
        public IActionResult Home()
        {
            var c = HttpContext.Request.Cookies;
            var r = Request.Cookies;
            var r2 = Response.Cookies;
            var u = User.Identity;
            return View();
        }

        private async void LogInUser(UserModel user)
        {
            List<Claim> personClaims = new()
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.EmailAddress)
                };

            List<ClaimsIdentity> claimsIdentities = new()
                {
                    new ClaimsIdentity(personClaims, "BugTracker.Auth.Identity")
                };

            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentities));
        }

        private UserModel GetLoggedInUserByEmail()
        {
            string email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            return _db.GetUser(email);
        }
        private bool IsValidEmailAddress(string emailAddress)
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