using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Bug_Tracker_Library;
using Bug_Tracker_Library.DataAccess;
using Bug_Tracker_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IDataAccessor _dataAccessor;

        public OrganizationController(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        // GET: Organization/OrganizationHome page, including a search for projects by name
        public IActionResult OrganizationHome(string searchString)
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
                Name = "Video Game",
                Priority = ProjectPriority.HIGH,
                Status = ProjectStatus.TODO,
                Deadline = Convert.ToDateTime("4/29/2021")
            };
            projectsUnfiltered.Add(p1);
            projectsUnfiltered.Add(p2);

            // Take the unfiltered projects and filter them by searched name
            List<ProjectModel> projects = new List<ProjectModel>();

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

            projects.OrderBy(x => x.Deadline); // order projects by deadline date

            ProjectsListViewModel model = new ProjectsListViewModel
            {
                Projects = projects,
                User = new UserModel { FirstName = "Andre", LastName = "Test" },
                UserPosition = UserPosition.WORKER
            };

            return View(model);
        }

        // GET: Organization/UserProfile/5
        public IActionResult UserProfile(int id)
        {
            return View();
        }

        // POST: Organization/UserProfile (updates user profile)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserProfile(IFormCollection collection)
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

        // GET: Organization/CreateOrganization
        public IActionResult CreateOrganization()
        {
            return View();
        }

        // POST: Organization/CreateOrganization
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrganization(IFormCollection collection)
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

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}