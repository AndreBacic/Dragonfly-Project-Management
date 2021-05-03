using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bug_Tracker_Library.DataAccess
{
    public class SQLDataAccessor : IDataAccessor
    {
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

        public void CreateProject(ProjectModel model, int organizationId)
        {
            throw new NotImplementedException();
        }
        public void CreateProject(ProjectModel model, Guid organizationId)
        {
            throw new NotImplementedException("This overload does not work for SQL data access. Use CreateProject(..., int organizationId).");
        }

        public void CreateUser(UserModel model)
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
    }
}