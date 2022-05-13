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
            ProjectViewModel projV = new()
            {
                Project = new ProjectModel()
            };
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
                return ProjectHome(model.Project.Id);
            }

            //_db.UpdateProject(model.Project, ?); // TODO: Add update logic here

            return ProjectHome(model.Project.Id);
        }

        // GET: Project/CreateProject
        public IActionResult CreateProject()
        {
            return View();
        }

        // POST: Project/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProject(CreateProjectModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(new CreateProjectModel()); // todo: tell user why the input is invalid.
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