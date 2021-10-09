using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Bug_Tracker_Library;
using Bug_Tracker_Library.DataAccess;
using Bug_Tracker_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    [Authorize]
    public class OrganizationController : Controller
    {
        private readonly IDataAccessor _db;

        public OrganizationController(IDataAccessor db)
        {
            _db = db;
        }

        // GET: Organization/OrganizationHome page, including a search for projects by name
        public IActionResult OrganizationHome(string searchString = null)
        {
            if (searchString != null)
            {
                searchString = searchString.ToLower(); // so comparisaons aren't case-sensitive
            }
            List<ProjectModel> projectsUnfiltered = new List<ProjectModel>();

            // TODO: Add data access logic here
            ProjectModel p1 = new ProjectModel
            {
                Name = "JavaScript project",
                Priority = ProjectPriority.MEDIUM,
                Status = ProjectStatus.TODO,
                Deadline = Convert.ToDateTime("6/5/2021")
            };
            ProjectModel p2 = new ProjectModel
            {
                Name = "Happy Burgers",
                Priority = ProjectPriority.HIGH,
                Status = ProjectStatus.TODO,
                Deadline = Convert.ToDateTime("4/29/2021")
            };
            projectsUnfiltered.Add(p1);
            projectsUnfiltered.Add(p2);

            // Take the unfiltered projects and filter them by searched name
            List<ProjectModel> projects = new();

            if (searchString != null)
            {
                foreach (ProjectModel p in projectsUnfiltered)
                {
                    if (p.Name.ToLower().Contains(searchString))
                    {
                        projects.Add(p);
                    }
                }
            }
            else
            {
                projects = projectsUnfiltered;
            }

            projects = projects.OrderBy(x => x.Deadline).ToList(); // order projects by deadline date

            ProjectsListViewModel model = new()
            {
                Projects = projects,
                User = new UserModel { FirstName = "Andre", LastName = "Test" },
                UserPosition = UserPosition.WORKER
            };

            return View(model);
        }

        // GET: Organization/CreateOrganization
        public IActionResult CreateOrganization()
        {
            return View();
        }

        // POST: Organization/CreateOrganization
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrganization(OrganizationViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(OrganizationHome));
            }
            catch
            {
                return View();
            }
        }

        [Authorize("Organization_ADMIN_policy")]
        // GET: Organization/CreateOrganization
        public IActionResult UpdateOrganization()
        {
            return View();
        }

        // POST: Organization/CreateOrganization
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Organization_ADMIN_policy")]
        public IActionResult UpdateOrganization(OrganizationViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(OrganizationHome));
            }
            catch
            {
                return View();
            }
        }

        //// Uncomment all of this if you need to make a delete organization page
        //// GET: Organization/DeleteOrganization/5
        //public IActionResult DeleteOrganization(int id)
        //{
        //    return View();
        //}

        //// POST: Organization/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteOrganization(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(OrganizationHome));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        /// <summary>
        /// This just handles the error message page
        /// </summary>
        /// <returns>A web page displaying an error message</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}