using Bug_Tracker_Library;
using Bug_Tracker_Library.DataAccess.MongoDB;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbSeederAndTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MongoDBDataAccessor mongodb = new("BugTracker");

            //CreateUserModel(mongodb);

            //CreateOrgWithAFewUsers(mongodb);

            UserModel user = mongodb.GetAllUsers()[5];
            OrganizationModel org = mongodb.LoadRecords<OrganizationModel>("Organizations")[0];
            //org = CreateProj(mongodb, org);
            //AssignUserToOrg1stProj(mongodb, user, org);

            Dictionary<Guid, UserModel> o = mongodb.GetAllOrganizationUsers(org.Id);

            string p = o.First().Value.Assignments[0].ProjectIdTreePath.ToString();
            Console.WriteLine(p);
        }

        private static OrganizationModel CreateProj(MongoDBDataAccessor mongodb, OrganizationModel org)
        {
            mongodb.CreateProject(new ProjectModel()
            {
                Name = "Auth work",
                Status = ProjectStatus.IN_PROGRESS
            }, org.Id);
            org = mongodb.LoadRecords<OrganizationModel>("Organizations")[0];
            return org;
        }

        private static void AssignUserToOrg1stProj(MongoDBDataAccessor mongodb, UserModel user, OrganizationModel org)
        {
            ProjectModel p = org.Projects[0];

            mongodb.CreateAssignment(new AssignmentModel()
            {
                AssigneeAccess = UserPosition.WORKER,
                AssigneeId = user.Id,
                OrganizationId = org.Id,
                ProjectIdTreePath = new List<Guid>() { p.Id }
            });
            p.WorkerIds.Add(user.Id);
            mongodb.UpdateProject(p, org.Id);
        }

        private static void CreateOrgWithAFewUsers(MongoDBDataAccessor mongodb)
        {
            System.Collections.Generic.List<Guid> ids = mongodb.GetAllUsers().Select(x => x.Id).ToList();

            ids.RemoveAt(4);
            ids.RemoveAt(1);

            mongodb.CreateOrganization(new OrganizationModel()
            {
                Name = "Realm of Lost Data",
                Description = "We're bad at devops",
                WorkerIds = ids
            });
        }

        private static void CreateUserModel(MongoDBDataAccessor mongodb)
        {
            UserModel user = new()
            {
                EmailAddress = "jane@jane.com",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "123-456-7890",
                PasswordHash = HashAndSalter.HashAndSalt("Anonymous").ToDbString()
            };

            mongodb.CreateUser(user);
        }
    }
}
