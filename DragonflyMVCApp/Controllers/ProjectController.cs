using DragonflyDataLibrary.DataAccess;
using DragonflyMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonflyMVCApp.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IDataAccessor _db;

        public ProjectController(IDataAccessor db)
        {
            _db = db;
        }
        public IActionResult Backlog()
        {
            Guid id = (Guid)RouteData.Values["id"];
            return View();
        }
        // GET: Project/ProjectHome page, with edit boxes and subproject links
        public IActionResult Backlog(int projectId)
        {
            return View();
        }

        // POST: Project/ProjectHome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectHome(CreateProjectViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                // TODO: get the project id and redirect
                return Backlog(0);
            }

            // TODO: Add update logic here

            return Backlog(0);
        }

        // GET: Project/CreateProject
        [HttpGet]
        public IActionResult DeleteProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Backlog));
            }
            catch
            {
                return View();
            }
        }
    }
}