using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;
using System;
using System.Collections.Generic;

namespace DbSeederAndTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MongoDBDataAccessor mongodb = new("Dragonfly");

            //CreateUserModel(mongodb);

            //UserModel user = mongodb.GetAllUsers()[5];
            //org = CreateProj(mongodb, org);
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
