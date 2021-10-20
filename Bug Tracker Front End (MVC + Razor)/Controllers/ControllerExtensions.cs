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
            string email = @this.User.ClaimValue(UserClaimsIndex.Email);
            return db.GetUser(email);
        }

        public static OrganizationModel GetLoggedInUsersOrganization(this Controller @this, IDataAccessor db, UserModel user = null)
        {
            string orgClaimId = @this.User.ClaimValue(UserClaimsIndex.OrganizationModel);
            Guid id = new(orgClaimId);
            return db.GetOrganization(id);
        }

        public static AssignmentModel GetLoggedInUsersAssignment(this Controller @this, IDataAccessor db, UserModel user = null)
        {
            string projectIdPath = @this.User.ClaimValue(UserClaimsIndex.ProjectModel);
            if (user is null)
            {
                user = @this.GetLoggedInUserByEmail(db);
            }
            return user.Assignments.FirstOrDefault(a =>
                string.Equals(a.ProjectIdTreePath.ListToString(), projectIdPath));
        }

        public static string ClaimValue(this ClaimsPrincipal @this, UserClaimsIndex index)
        {
            return @this.Claims.ToList()[(int)index].Value;
        }
    }
}
