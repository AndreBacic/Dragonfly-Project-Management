using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library.DataAccess
{
    public interface IDataAccessor
    {
        void CreateProject(ProjectModel model);
        void CreateUser(UserModel model);
        void CreateComment(CommentModel model);
        void CreateOrganization(OrganizationModel model);
        void CreasteAssignment(AssignmentModel model);
        /// <summary>
        /// Gets an organizationModel by its name and password
        /// This method also gets all the projects, users, assignments, and comments in the organization
        /// </summary>
        /// <param name="organizationName">User must enter the name of the organization to gain access</param>
        /// <param name="password">User must also enter the organization password to gain access</param>
        /// <returns>The fully filled out organization</returns>
        OrganizationModel GetOrganization(string organizationName, string password);
        UserModel GetUser(string userName, string password);
    }
}
