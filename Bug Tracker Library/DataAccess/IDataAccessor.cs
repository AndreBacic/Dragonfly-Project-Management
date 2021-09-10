using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess
{
    /// <summary>
    /// Interface for a data access class
    /// NOTE: implementations should have a constructor with parameter (IConfiguration configuration)
    ///                         where configuration is for grabbing connection string
    /// </summary>
    public interface IDataAccessor
    {
        void CreateProject(ProjectModel model, Guid organizationId, List<int> indexTree);
        /// <summary>
        /// Inserts model into the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether model could be successfully entered. It cannot be successfully entered if the password of the user is not unique.</returns>
        bool CreateUser(UserModel model);
        /// <summary>
        /// Adds model to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="organizationId"></param>
        /// <param name="indexTree">ProjectModel indecies for navigating the subproject tree</param>
        void CreateComment(CommentModel model, Guid organizationId, List<int> indexTree);
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

        List<UserModel> GetAllUsers();
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
