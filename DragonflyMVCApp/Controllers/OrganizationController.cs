using DragonflyMVCApp.Models;
using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DragonflyMVCApp.Controllers
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

            UserModel user = this.GetLoggedInUserByEmail(_db);

            OrganizationHomeModel model = new()
            {
                Organization = org,
                User = user,
                UserAssignment = this.GetLoggedInUsersAssignment(_db, user)
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
        public IActionResult CreateOrganization(CreateOrganizationModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            OrganizationModel org = new() {
                Name = model.Name,
                Description = model.Description,
                WorkerIds = new List<Guid>() { model.CreatorId }
            };

            bool didCreate = _db.CreateOrganization(org);

            if (didCreate == false)
            {
                return View(); // TODO: Tell user that the name is taken.
            }

            org = _db.GetOrganization(model.Name); // fetch mongo generated id.

            AssignmentModel assignment = new()
            {
                AssigneeId = model.CreatorId,
                AssigneeAccess = UserPosition.ADMIN,
                OrganizationId = org.Id,
                ProjectIdTreePath = null,
                HoursLogged = 0
            };

            _db.CreateAssignment(assignment);

            return RedirectToAction(nameof(AccountController.Home), "Account", assignment);
        }

        [Authorize("Organization_ADMIN_policy")]
        // GET: Organization/CreateOrganization
        public IActionResult ManageOrganization()
        {
            UserModel user = this.GetLoggedInUserByEmail(_db);
            ManageOrganizationModel mog = new()
            {
                Organization = this.GetLoggedInUsersOrganization(_db, user),
                LoggedInUser = user,
                DidUpdateOrg = ""
            };
            mog.Workers = _db.GetAllOrganizationUsers(mog.Organization.Id);
            return View(mog);
        }

        // POST: Organization/CreateOrganization
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Organization_ADMIN_policy")]
        public IActionResult ManageOrganization(EditOrganizationModel model)
        {
            ManageOrganizationModel mog = new()
            {
                Organization = _db.GetOrganization(model.Id),
                LoggedInUser = this.GetLoggedInUserByEmail(_db)
            };
            mog.Workers = _db.GetAllOrganizationUsers(mog.Organization.Id);

            if (ModelState.IsValid == false)
            {
                mog.DidUpdateOrg = "False";
                return View(mog);
            }

            mog.Organization.Description = model.Description;
            mog.Organization.Name = model.Name;

            mog.DidUpdateOrg = _db.UpdateOrganization(mog.Organization).ToString();

            return View(mog);
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