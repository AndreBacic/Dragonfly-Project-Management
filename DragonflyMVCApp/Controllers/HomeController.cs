using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;
using DragonflyMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

#nullable enable

namespace DragonflyMVCApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDataAccessor _db;

        public HomeController(IDataAccessor db)
        {
            _db = db;
        }

        // GET: HomeController
        public IActionResult Home()
        {
            var user = _db.GetUser(User.ClaimValue(UserClaimsIndex.Email));
            return View(user);
        }
        // POST: HomeController search for project by title or description
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Home(string? search)
        {
            var user = _db.GetUser(User.ClaimValue(UserClaimsIndex.Email));
            if (search is null) return View(user);

            search = search.ToLower();
            user.Projects = user.Projects
                .Where(p => p.Title.ToLower().Contains(search) ||
                            p.Description.ToLower().Contains(search)).ToList();
            return View(user);
        }

        // GET: HomeController/CreateProject
        public IActionResult CreateProject()
        {
            return View();
        }

        // POST: HomeController/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProject(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Deadlines/
        public IActionResult Deadlines()
        {
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult EditAccount(ChangePasswordViewModel? data = null)
        {
            UserModel user = this.GetLoggedInUserByEmail(_db);

            if (data is null) data = new();

            var model = new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                ChangePasswordView = data
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditUserViewModel updatedUser)
        {
            if (ModelState.IsValid == false)
            {
                return View(updatedUser);
            }

            bool emailTaken = _db.GetUser(updatedUser.EmailAddress) is null;
            if (emailTaken)
            {
                ModelState.AddModelError("EmailAddress", "That email address is already taken.");
                return View(updatedUser);
            }

            UserModel loggedInUser = this.GetLoggedInUserByEmail(_db);

            loggedInUser.FirstName = updatedUser.FirstName;
            loggedInUser.LastName = updatedUser.LastName;
            loggedInUser.EmailAddress = updatedUser.EmailAddress;
            // TODO: Add color preference toggle
            //loggedInUser.ColorPreference = updatedUser.ColorPreference;
            _db.UpdateUser(loggedInUser);

            this.LogInUser(loggedInUser);

            return RedirectToAction(nameof(EditAccount)); // TODO: Add success message
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(EditAccount), data);
            }
            UserModel dbUser = this.GetLoggedInUserByEmail(_db);
            PasswordHashModel passwordHash = new();
            passwordHash.FromDbString(dbUser.PasswordHash);

            (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(data.OldPassword, passwordHash);
            if (IsPasswordCorrect == false)
            {
                ModelState.AddModelError("OldPassword", "Old password is incorrect");
                return RedirectToAction(nameof(EditAccount), data);
            }

            dbUser.PasswordHash = HashAndSalter.HashAndSalt(data.NewPassword).ToDbString();
            _db.UpdateUser(dbUser);

            this.LogInUser(dbUser);

            return RedirectToAction(nameof(EditAccount), data); // TODO: Add success message
        }
    }
}