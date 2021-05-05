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
        /// <summary>
        /// Inserts model into the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether model could be successfully entered. It cannot be successfully entered if the name and password of the organization are not unique.</returns>
        bool CreateOrganization(OrganizationModel model);
        void CreasteAssignment(AssignmentModel model);
        /// <summary>
        /// Gets an organizationModel by its name and password
        /// This method also gets all the projects, users, assignments, and comments in the organization
        /// </summary>
        /// <param name="organizationName">User must enter the name of the organization to gain access</param>
        /// <param name="passwordHash">User must also enter the organization password to gain access. Password must already be properly hashed.</param>
        /// <returns>The fully filled out organization</returns>
        OrganizationModel GetOrganization(string organizationName, string passwordHash);

        /// <summary>
        /// Gets the user with Id of id. This overload is for sql/other database types.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUser(int id);
        /// <summary>
        /// Gets the user with Id of id. This overload is for MongoDB or similar database types that use Guid ids.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUser(Guid id);
        /// <summary>
        /// Updates model in the database.
        /// </summary>
        /// <param name="model"></param>
        void UpdateOrganization(OrganizationModel model);
        void UpdateUser(UserModel model);
    }
}
