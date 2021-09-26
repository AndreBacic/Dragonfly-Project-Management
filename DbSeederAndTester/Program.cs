using Bug_Tracker_Library.DataAccess.MongoDB;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using System;
using System.Linq;

namespace DbSeederAndTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MongoDBDataAccessor mongodb = new MongoDBDataAccessor("BugTracker");

            //CreateUserModel(mongodb);

            //CreateOrgWithAFewUsers(mongodb);

            System.Collections.Generic.Dictionary<Guid, UserModel> o = mongodb.GetAllOrganizationUsers(
                mongodb.LoadRecords<OrganizationModel>("Organizations")[0].Id);
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
            UserModel user = new UserModel()
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
