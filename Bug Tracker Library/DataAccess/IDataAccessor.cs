using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.DataAccess
{
    /// <summary>
    /// Interface for a data access class
    /// <br></br>
    /// NOTE: implementations should have a constructor with parameter (IConfiguration configuration)
    /// where configuration is for grabbing connection string
    /// </summary>
    public interface IDataAccessor
    {
        void CreateProject(ProjectModel model, Guid organizationId);
        /// <summary>
        /// Inserts model into the database if model.EmailAddress is unique.
        /// <br/>
        /// Assumes model.PasswordHash has been hashed properly.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether model could be successfully entered.</returns>
        bool CreateUser(UserModel model);
        /// <summary>
        /// Adds model to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="organizationId"></param>
        /// <param name="indexTree">ProjectModel indecies for navigating the subproject tree</param>
        void CreateComment(CommentModel model, Guid organizationId);
        /// <summary>
        /// Inserts model into the database if model.Name is unique.
        /// <br/>
        /// Assumes model.PasswordHash has been hashed properly.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether model could be successfully entered. </returns>
        bool CreateOrganization(OrganizationModel model);
        /// <summary>
        /// Adds model to the database.
        /// two assignments may not point to the same project.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CreateAssignment(AssignmentModel model);
        /// <summary>
        /// Gets an OrganizationModel by its id,
        /// with all of the projects, users, assignments, and comments in the organization.
        /// </summary>
        /// <param name="organizationName">User must enter the name of the organization to gain access</param>
        /// <param name="passwordHash">User must also enter the organization password to gain access. Password must already be properly hashed.</param>
        /// <returns>The fully filled out organization</returns>
        OrganizationModel GetOrganization(Guid id);
        /// <summary>
        /// Gets an OrganizationModel by its unique name and password,
        /// with all of the projects, users, assignments, and comments in the organization.
        /// </summary>
        /// <param name="organizationName">User must enter the name of the organization to gain access</param>
        /// <param name="passwordHash">User must also enter the organization password to gain access. Password must already be properly hashed.</param>
        /// <returns>The fully filled out organization</returns>
        OrganizationModel GetOrganization(string organizationName, PasswordHashModel passwordHash);

        List<UserModel> GetAllUsers();
        /// <summary>
        /// Gets the user with Id of id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUser(Guid id);
        /// <summary>
        /// Gets the user with unique EmailAddress of parameter emailAddress.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        UserModel GetUser(string emailAddress, PasswordHashModel passwordHash);

        /// <summary>
        /// Updates model in the database.
        /// </summary>
        /// <param name="model"></param>
        void UpdateOrganization(OrganizationModel model);
        void UpdateUser(UserModel model);
        void UpdateProject(ProjectModel model, Guid organizationId);
        void UpdateComment(CommentModel model, Guid organizationId);
        void UpdateAssignment(AssignmentModel model);

        void DeleteOrganization(OrganizationModel model);
        void DeleteUser(UserModel model);
        void DeleteProject(ProjectModel model, Guid organizationId);
        void DeleteComment(CommentModel model, Guid organizationId);
        void DeleteAssignment(AssignmentModel model);
    }
}
