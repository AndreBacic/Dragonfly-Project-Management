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

        public IActionResult EditAccount()
        {
            UserModel user = this.GetLoggedInUserByEmail(_db);

            return View(user.DbUserToEditView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccount(EditUserViewModel updatedUser)
        {
            //bool emailTaken = _db.GetUser(updatedUser.EmailAddress) is null;
            UserModel loggedInUser = this.GetLoggedInUserByEmail(_db);

            // TODO: Finish this edit account logic

            return View();
        }

        // Change password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel data)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Show error messages
                ViewData["ChangePasswordError"] = "Invalid inputs";
                return EditAccount();
            }
            UserModel dbUser = this.GetLoggedInUserByEmail(_db);
            PasswordHashModel passwordHash = new();
            passwordHash.FromDbString(dbUser.PasswordHash);

            (bool IsPasswordCorrect, _) = HashAndSalter.PasswordEqualsHash(data.OldPassword, passwordHash);
            if (IsPasswordCorrect == false)
            {
                ModelState.AddModelError("OldPassword", "Old password is incorrect");
            }
            
            dbUser.PasswordHash = HashAndSalter.HashAndSalt(data.NewPassword).ToDbString();
            _db.UpdateUser(dbUser);

            return EditAccount();
        }
    }
}