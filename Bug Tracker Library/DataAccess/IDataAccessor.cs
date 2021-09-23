using Bug_Tracker_Library.Models;
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
        void CreateProject(IProjectModel model, Guid organizationId);
        /// <summary>
        /// Inserts model into the database if model.EmailAddress is unique.
        /// <br/>
        /// Assumes model.PasswordHash has been hashed properly.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether model could be successfully entered.</returns>
        bool CreateUser(IUserModel model);
        /// <summary>
        /// Adds model to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="organizationId"></param>
        void CreateComment(ICommentModel model, Guid organizationId);
        /// <summary>
        /// Inserts model into the database if model.Name is unique.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Whether model could be successfully entered. </returns>
        bool CreateOrganization(IOrganizationModel model);
        /// <summary>
        /// Adds model to the database.
        /// two assignments may not point to the same project.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CreateAssignment(IAssignmentModel model);
        /// <summary>
        /// Gets an IOrganizationModel by its id,
        /// with all of the projects and comments in the organization.
        /// </summary>
        IOrganizationModel GetOrganization(Guid id);

        List<IUserModel> GetAllUsers();
        IUserModel GetUser(Guid id);
        /// <summary>
        /// Gets the user with unique EmailAddress of parameter emailAddress.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IUserModel GetUser(string emailAddress, string password);

        void UpdateOrganization(IOrganizationModel model);
        void UpdateUser(IUserModel model);
        void UpdateProject(IProjectModel model, Guid organizationId);
        void UpdateComment(ICommentModel model, Guid organizationId);
        void UpdateAssignment(IAssignmentModel model);

        void DeleteOrganization(IOrganizationModel model);
        void DeleteUser(IUserModel model);
        void DeleteProject(IProjectModel model, Guid organizationId);
        void DeleteComment(ICommentModel model, Guid organizationId);
        void DeleteAssignment(IAssignmentModel model);
    }
}
