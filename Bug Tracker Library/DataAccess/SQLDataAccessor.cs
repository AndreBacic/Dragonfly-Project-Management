using Bug_Tracker_Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bug_Tracker_Library.DataAccess
{
    public class SQLDataAccessor : IDataAccessor
    {
        private UserModel _user;
        private OrganizationModel _organization;

        public SQLDataAccessor(IConfiguration configuration, UserModel user, OrganizationModel organization)
        {
            // TODO: Actually have this whole class do something
            
            // _user and _organization are passed by reference so this can track their changes and save them easily
            _user = user;
            _organization = organization;
        }
        public void SetGlobalUser(UserModel user)
        {
            this._user = user;
        }

        public void SetGlobalOrganization(OrganizationModel organization)
        {
            this._organization = organization;
        }
        public void CreasteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(CommentModel model)
        {
            throw new NotImplementedException();
        }

        public bool CreateOrganization(OrganizationModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateProject(ProjectModel model)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(UserModel model)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel GetOrganization(string organizationName, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(Guid id)
        {
            throw new NotImplementedException("This overload does not work for SQL data access. Use GetUser(int id).");
        }

        public void UpdateOrganization(OrganizationModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}