using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    public class ProjectController : Controller
    {
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Uncomment this stuff if you need to add a delete project page
        //// GET: Project/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Project/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}