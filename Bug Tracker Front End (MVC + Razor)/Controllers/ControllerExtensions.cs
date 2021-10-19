using Bug_Tracker_Library;
using Bug_Tracker_Library.DataAccess;
using Bug_Tracker_Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
namespace Bug_Tracker_Front_End__MVC_plus_Razor.Controllers
{
    public static class ControllerExtensions
    {
        public static UserModel GetLoggedInUserByEmail(this Controller @this, IDataAccessor db)
        {
            string email = @this.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            return db.GetUser(email);
        }

        public static OrganizationModel GetLoggedInUsersOrganization(this Controller @this, IDataAccessor db, UserModel user = null)
        {
            string orgClaimId = @this.User.Claims.ToList()[(int)UserClaimsIndex.OrganizationModel].Value;
            Guid id = new(orgClaimId);
            return db.GetOrganization(id);
        }

        public static AssignmentModel GetLoggedInUsersAssignment(this Controller @this, IDataAccessor db, UserModel user = null)
        {
            string projectIdPath = @this.User.Claims.ToList()[(int)UserClaimsIndex.ProjectModel].Value;
            if (user is null)
            {
                user = @this.GetLoggedInUserByEmail(db);
            }
            return user.Assignments.FirstOrDefault(a =>
                string.Equals(a.ProjectIdTreePath.ListToString(), projectIdPath));
        }
    }
}
