using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library;
using Bug_Tracker_Front_End__MVC_plus_Razor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project list sorted by searchString
        public ActionResult Index(string searchString)
        {
            if (searchString != null)
            {
                searchString = searchString.ToLower(); // so comparisaons aren't case-sensitive
            }
            List<ProjectModel> projectsUnfiltered = new List<ProjectModel>();

            // TODO: Add data access logic here
            ProjectModel p1 = new ProjectModel { Name = "JavaScript project", Priority = ProjectPriority.MEDIUM, 
                Status = ProjectStatus.TODO, Deadline = Convert.ToDateTime("6/5/2021") };
            ProjectModel p2 = new ProjectModel { Name = "Video Game", Priority = ProjectPriority.HIGH, 
                Status = ProjectStatus.TODO, Deadline = Convert.ToDateTime("4/29/2021") };
            projectsUnfiltered.Add(p1);
            projectsUnfiltered.Add(p2);

            UserModel u1 = new UserModel { FirstName = "Andre", LastName = "Bacic" };

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

            ProjectsListViewModel model = new ProjectsListViewModel { Projects=projects, User=u1, UserPosition=UserPosition.WORKER };

            return View(model);
        }

        // GET: Project/CreateProject
        public ActionResult CreateProject()
        {
            return View();
        }

        // POST: Project/CreateProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(IFormCollection collection)
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

        // GET: Project/ProjectHome/5
        public ActionResult ProjectHome(int id)
        {
            return View();
        }

        // POST: Project/ProjectHome/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectHome(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Uncomment this stuff if you need to add a delete project page
        //// GET: Project/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Project/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
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