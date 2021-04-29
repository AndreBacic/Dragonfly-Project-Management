using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library.DataAccess
{
    public class MongoDBDataAccessor : IDataAccessor
    {
        public void CreasteAssignment(AssignmentModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateComment(CommentModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateOrganization(OrganizationModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateProject(ProjectModel model)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserModel model)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel GetOrganization(string organizationName, string password)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
