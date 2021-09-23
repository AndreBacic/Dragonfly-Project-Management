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
                IUserModel dbUser = _db.GetAllUsers()
                    .Where(u => u.EmailAddress == user.EmailAddress).First();

                PasswordHashModel passwordHash = new PasswordHashModel();
                passwordHash.FromDbString(dbUser.PasswordHash);

                (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(user.Password, passwordHash);

                if (IsPasswordCorrect)
                {
                    LogInUser(dbUser);

                    string t = HttpContext.User.Identity.Name;

                    return RedirectToAction("Index", "Todo");
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

        [Authorize("Auth_Policy")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
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

            List<IUserModel> allUsers = _db.GetAllUsers();
            if (allUsers.Any(x => x.EmailAddress == newUser.EmailAddress))
            {
                ViewData["RegisterMessage"] = "That email address is taken";
                return View();
            }
            IUserModel newDbUser = new IUserModel
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                EmailAddress = newUser.EmailAddress,
                PasswordHash = HashAndSalter.HashAndSalt(newUser.Password).ToDbString()
            };
            _db.CreateUser(newDbUser);

            LogInUser(newDbUser);

            return RedirectToAction("Index", "Todo");
        }

        [Authorize("Auth_Policy")]
        public IActionResult EditAccount()
        {
            IUserModel user = GetLoggedInUserByEmail();

            return View(DbUserToEditView(user));
        }
        [Authorize("Auth_Policy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditUserViewModel updatedUser)
        {
            // 1) Make sure email isn't taken
            List<IUserModel> allUsers = _db.GetAllUsers();
            IUserModel loggedInUser = GetLoggedInUserByEmail();

            if (IsValidEmailAddress(updatedUser.EmailAddress) == false ||
                allUsers.Any(x => x.EmailAddress == updatedUser.EmailAddress && updatedUser.EmailAddress != loggedInUser.EmailAddress))
            {
                ViewData["EditMessage"] = "That email address is taken"; // todo: refactor this viewdata message system
                return View(DbUserToEditView(loggedInUser));
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
                    return RedirectToAction("Index", "Todo");
                }
                else
                {
                    return View(DbUserToEditView(loggedInUser));
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

                return RedirectToAction("Index", "Todo");
            }
        }

        private async void LogInUser(IUserModel user)
        {
            List<Claim> personClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.EmailAddress)
                };

            List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(personClaims, "TodoAuth.Identity")
                };

            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentities));
        }

        private IUserModel GetLoggedInUserByEmail()
        {
            string email = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Email).First().Value;
            return _db.GetAllUsers().Where(x => x.EmailAddress == email).First();
        }
        private bool IsValidEmailAddress(string emailAddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private EditUserViewModel DbUserToEditView(IUserModel user)
        {
            return new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };
        }
    }
}