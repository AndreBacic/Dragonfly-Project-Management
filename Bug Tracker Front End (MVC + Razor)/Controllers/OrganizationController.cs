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
using System.Security.Claims;

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

            OrganizationModel org = _db.GetOrganization(
                new Guid(User.Claims.ToList()[(int)UserClaimsIndex.OrganizationModel].Value));

            org.Projects = org.Projects
                .Where(p => p.Name.ToLower().Contains(searchString))
                .OrderBy(x => x.Deadline)
                .ToList();

            UserModel user = GetLoggedInUserByEmail();

            OrganizationHomeModel model = new()
            {
                Organization = org,
                User = user,
                UserAssignment = GetLoggedInUsersAssignment(user)
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
            if (ModelState.IsValid == false)
            {
                return View();
            }

            OrganizationModel org = new() {
                Name = model.Name,
                Description = model.Description,
                WorkerIds = new List<Guid>() { model.CreatorOrEditorId }
            };

            bool didCreate = _db.CreateOrganization(org);

            if (didCreate == false)
            {
                return View(); // TODO: Tell user that the name is taken.
            }

            org = _db.GetOrganization(model.Name); // fetch mongo generated id.

            _db.CreateAssignment(new AssignmentModel()
            {
                AssigneeId = model.CreatorOrEditorId,
                AssigneeAccess = UserPosition.ADMIN,
                OrganizationId = org.Id
            });

            return RedirectToAction(nameof(OrganizationHome));
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

        private UserModel GetLoggedInUserByEmail()
        {
            string email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            return _db.GetUser(email);
        }

        private AssignmentModel GetLoggedInUsersAssignment(UserModel user = null)
        {
            string projectIdPath = User.Claims.ToList()[(int)UserClaimsIndex.ProjectModel].Value;
            if (user is null)
            {
                user = GetLoggedInUserByEmail();
            }
            return user.Assignments.FirstOrDefault(a => 
                string.Equals(a.ProjectIdTreePath.ListToString(), projectIdPath));
        }
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