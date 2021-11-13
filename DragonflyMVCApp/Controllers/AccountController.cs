using DragonflyMVCApp.Models;
using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                UserModel dbUser = _db.GetUser(user.EmailAddress);

                PasswordHashModel passwordHash = new();
                passwordHash.FromDbString(dbUser.PasswordHash);

                (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(user.Password, passwordHash);

                if (IsPasswordCorrect)
                {
                    LogInUser(dbUser);

                    return RedirectToAction(nameof(Home));
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
            UserModel user = this.GetLoggedInUserByEmail(_db);

            return View(user.DbUserToEditView());
        }
        [Authorize("Logged_in_user_policy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditUserViewModel updatedUser)
        {
            // 1) Make sure email isn't taken
            List<UserModel> allUsers = _db.GetAllUsers();
            UserModel loggedInUser = this.GetLoggedInUserByEmail(_db);

            if (IsValidEmailAddress(updatedUser.EmailAddress) == false ||
                allUsers.Any(x => x.EmailAddress == updatedUser.EmailAddress && updatedUser.EmailAddress != loggedInUser.EmailAddress))
            {
                ViewData["EditMessage"] = "That email address is taken"; // todo: refactor this viewdata message system
                return View(loggedInUser.DbUserToEditView());
            }

            if (string.IsNullOrWhiteSpace(updatedUser.NewPassword) == false)
            {
                // 2) Make sure old password is correct
                PasswordHashModel passwordHash = new();
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
            // todo: replace manual redirect with better auth policies/schemes?
            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        public IActionResult MyProjects()
        {
            LogInUser(this.GetLoggedInUserByEmail(_db),
                new AssignmentModel()
                { 
                    AssigneeAccess = UserPosition.ADMIN,
                    OrganizationId = Guid.NewGuid(), 
                    ProjectIdTreePath = new List<Guid>() { new Guid() } 
                });
            return View();
        }

        [Authorize("Logged_in_user_policy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Home(AssignmentModel assignment)
        {
            // TODO: Have user select assignment and direct them to either org home or project home
            UserModel u = this.GetLoggedInUserByEmail(_db);
            LogInUser(u, assignment);
            return View();
        }
        private async void LogInUser(UserModel user)
        {
            List<Claim> personClaims = new()
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(nameof(user.Id), user.Id.ToString())
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(personClaims, "Dragonfly.Auth.Identity")));
        }

        private async void LogInUser(UserModel user, AssignmentModel assignment)
        {
            List<Claim> personClaims = new()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(nameof(user.Id), user.Id.ToString()),
                new Claim(ClaimTypes.Role, assignment.AssigneeAccess.ToString()),
                new Claim(nameof(OrganizationModel), assignment.OrganizationId.ToString()),
                new Claim(nameof(ProjectModel), assignment.ProjectIdTreePath.ListToString())
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