using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess.MongoDB;
using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbSeederAndTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MongoDBDataAccessor mongodb = new("Dragonfly");

            //CreateUserModel(mongodb);

            //CreateOrgWithAFewUsers(mongodb);

            UserModel user = mongodb.GetAllUsers()[5];
            //org = CreateProj(mongodb, org);
            //AssignUserToOrg1stProj(mongodb, user, org);

            Dictionary<Guid, UserModel> o = mongodb.GetAllOrganizationUsers(org.Id);

        }

        private static void CreateUserModel(MongoDBDataAccessor mongodb)
        {
            UserModel user = new()
            {
                EmailAddress = "jane@jane.com",
                FirstName = "Jane",
                LastName = "Doe",
                PasswordHash = HashAndSalter.HashAndSalt("Anonymous").ToDbString()
            };

            mongodb.CreateUser(user);
        }
    }
}
