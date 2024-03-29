﻿using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.Models;
using DragonflyDataLibrary.Security;

namespace DbSeederAndTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MongoDBDataAccessor mongodb = new("Dragonfly");

            //CreateUserModel(mongodb);

            //UserModel user = mongodb.GetAllUsers()[5];
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
