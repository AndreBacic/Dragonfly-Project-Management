using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Bug_Tracker_Library.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public IActionResult ProjectHome(List<Guid> projectIdTreePath)
        {
            Bug_Tracker_Library.Models.OrganizationModel org = this.GetLoggedInUsersOrganization(_db);
            Bug_Tracker_Library.Models.ProjectModel proj = org.GetProjectByIdTree(projectIdTreePath);
            ProjectViewModel projV = new()
            {
                Id = proj.Id,
                Name = proj.Name,
                Description = proj.Description,
                Deadline = proj.Deadline,
                Comments = proj.Comments,
                Priority = proj.Priority,
                Status = proj.Status,
                SubProjects = proj.SubProjects,
                ParentIdTreePath = proj.ParentIdTreePath
            };
            Dictionary<Guid, Bug_Tracker_Library.Models.UserModel> workers = _db.GetAllOrganizationUsers(org.Id);
            foreach (Guid id in proj.WorkerIds)
            {
                projV.Workers.Add(id, workers[id]);
            }
            return View(projV);
        }

        // POST: Project/ProjectHome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectHome(ProjectViewModel model)
        {
            try
            {
                // TODO: save edited project info here

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