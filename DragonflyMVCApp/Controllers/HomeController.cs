using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Home(string search)
        {
            search = search.ToLower();
            var user = _db.GetUser(User.ClaimValue(UserClaimsIndex.Email));
            user.Projects.RemoveAll(p => !p.Title.ToLower().Contains(search) &&
                                         !p.Description.ToLower().Contains(search));
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
    }
}
