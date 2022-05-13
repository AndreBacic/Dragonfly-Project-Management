using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public IActionResult ProjectHome(int projectId)
        {
            ProjectViewModel projV = new();
            return View(projV);
        }

        // POST: Project/ProjectHome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProjectHome(ProjectViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                // todo: get the project id and redirect
                return ProjectHome(0);
            }

            // TODO: Add update logic here

            return ProjectHome(0);
        }

        // GET: Project/CreateProject
        public IActionResult CreateProject()
        {
            return View();
        }

        // POST: Project/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProject(ProjectModel model)
        {
            if (ModelState.IsValid == false) // todo: use a view model to validate? Bind only desired properties?
            {
                return View(); // todo: tell user why the input is invalid.
            }
            // todo: add project to database, get generated id and redirect to project home page with id.
            int projectId = 1; //_db.InsertRecord<ProjectModel>(something);
            return ProjectHome(projectId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProject(List<Guid> projectIdTreePath)
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