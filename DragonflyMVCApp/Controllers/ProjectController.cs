using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
        
        // GET: Project/ProjectHome page, with edit boxes and subproject links
        public IActionResult Backlog(Guid id)
        {
            var proj = this.GetLoggedInUserByEmail(_db).Projects.FirstOrDefault(p => p.Id == id);
            return View(proj);
        }

        // POST: Project/ProjectHome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectHome(Guid id, [FromBody]ProjectModel model)
        {
            if (ModelState.IsValid == false)
            {
                // TODO: get the project id and redirect
                return Backlog(id);
            }
            
            // TODO: Add update logic here

            return Backlog(id);
        }

        // GET: Project/CreateProject
        [HttpGet]
        public IActionResult DeleteProject(Guid id)
        {
            var proj = this.GetLoggedInUserByEmail(_db).Projects.FirstOrDefault(p => p.Id == id);
            return View(proj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProject(Guid id, bool confirm)
        {
            var user = this.GetLoggedInUserByEmail(_db);
            try
            {
                if (confirm == true)
                {
                    user.Projects.RemoveAll(p => Equals(p.Id, id));
                    _db.UpdateUser(user);
                }

                return RedirectToAction(nameof(Backlog));
            }
            catch
            {
                return View(user.Projects.FirstOrDefault(p => p.Id == id));
            }
        }
    }
}