using Bug_Tracker_Library.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IDataAccessor _db;

        public ProjectController(IDataAccessor db)
        {
            _db = db;
        }
        // GET: Project/ProjectHome page, with edit boxes and subproject links
        public IActionResult ProjectHome()
        {
            // TODO: Finish this method
            return View();
        }

        // POST: Project/ProjectHome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectHome(IFormCollection collection)
        {
            try
            {
                // TODO: Add save edited project info here

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/CreateProject
        public IActionResult CreateProject()
        {
            return View();
        }

        // POST: Project/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProject(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(ProjectHome));
            }
            catch
            {
                return View();
            }
        }
                
        // GET: Project/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(ProjectHome));
            }
            catch
            {
                return View();
            }
        }
    }
}