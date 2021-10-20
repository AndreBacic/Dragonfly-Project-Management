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
                Project = proj
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
            if (ModelState.IsValid == false)
            {
                // todo: fix the ironic 'solution' of trying to fix a broken input based on a possibly broken input.
                return ProjectHome(model.Project.IdTreePath);
            }

            //_db.UpdateProject(model.Project, ?); // TODO: Add update logic here
            //_db.UpdateAssignment(change user assignments ?);

            return ProjectHome(model.Project.IdTreePath);
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