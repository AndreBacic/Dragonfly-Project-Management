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
        public ActionResult Home()
        {
            return View();
        }

        // GET: HomeController/CreateProject
        public ActionResult CreateProject()
        {
            return View();
        }

        // POST: HomeController/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(IFormCollection collection)
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
        public ActionResult Deadlines()
        {
            return View();
        }

        public ActionResult Analytics()
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
