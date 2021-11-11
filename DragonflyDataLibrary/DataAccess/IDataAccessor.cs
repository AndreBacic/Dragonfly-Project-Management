using DragonflyDataLibrary.Models;
using System;
using System.Collections.Generic;

namespace DragonflyDataLibrary.DataAccess
{
    /// <summary>
    /// Interface for a data access class
    /// <br/>
    /// NOTE: implementations should have a constructor with parameter (IConfiguration configuration)
    /// where configuration is for grabbing a connection string
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
        void CreateComment(CommentModel model, Guid organizationId);
        /// <summary>
        /// Inserts model into the database if model.Name is unique.
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
        /// with all of the projects and comments in the organization.
        /// </summary>
        OrganizationModel GetOrganization(Guid id);
        /// <summary>
        /// Gets an OrganizationModel by its unique name,
        /// with all of the projects and comments in the organization.
        /// </summary>
        OrganizationModel GetOrganization(string name);

        List<UserModel> GetAllUsers();
        Dictionary<Guid, UserModel> GetAllOrganizationUsers(Guid organizationId);
        UserModel GetUser(Guid id);
        /// <summary>
        /// Gets the user with unique EmailAddress of parameter emailAddress.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        UserModel GetUser(string emailAddress);
        /// <summary>
        /// Gets the user with unique EmailAddress of parameter emailAddress,
        /// and checks if the plaintext password is the correct password.
        /// <br/>
        /// If the password is correct, the user data will be returned as normal. Otherwise, null.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserModel GetUser(string emailAddress, string password);

        bool UpdateOrganization(OrganizationModel model);
        bool UpdateUser(UserModel model);
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
