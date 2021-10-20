using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Bug_Tracker_Library;
using Bug_Tracker_Library.DataAccess;
using Bug_Tracker_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
            OrganizationModel org = this.GetLoggedInUsersOrganization(_db);
            ProjectModel proj = org.GetProjectByIdTree(projectIdTreePath);
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
        public IActionResult CreateProject(CreateProjectModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(new CreateProjectModel()); // todo: tell user why the input is invalid.
            }

            Guid orgId = new(User.ClaimValue(UserClaimsIndex.OrganizationModel));

            ProjectModel proj = new()
            {
                Name = model.Name,
                Deadline = model.Deadline,
                Description = model.Description,
                ParentIdTreePath = model.ParentIdTreePath,
                Priority = model.Priority,
                Status = model.Status
            };
            _db.CreateProject(proj, orgId);

            _db.CreateAssignment(new AssignmentModel()
            {
                AssigneeAccess = UserPosition.ADMIN, // todo: no admins for projects? should the creator be a manager?
                AssigneeId = new Guid(User.ClaimValue(UserClaimsIndex.Id)),
                OrganizationId = orgId,
                ProjectIdTreePath = proj.IdTreePath
            });

            return ProjectHome(proj.IdTreePath);
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