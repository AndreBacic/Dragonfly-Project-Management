using Bug_Tracker_Library.DataAccess.MongoDB;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using System;
using System.Collections.Generic;
using Xunit;
// todo: put this back in it's original folder and figure out how to get the c# compiler to recoginize this program's Main.
namespace Bug_Tracker.Tests.Library.DataAccess
{
    public class MongoDBTests
    {
        //[Fact] // todo: remove depreciated test?
        public void MongoDbDataAccess_ShouldWork()
        {
            MongoDBDataAccessor mongodb = new MongoDBDataAccessor("BugTracker");

            //CRUDUserModel(mongodb);
        }

        private void CRUDUserModel(MongoDBDataAccessor mongodb)
        {
            UserModel user = new UserModel()
            {
                EmailAddress = "dre@dre.net",
                FirstName = "Dre",
                LastName = "AlsoDre",
                PhoneNumber = "123-456-7890",
                PasswordHash = HashAndSalter.HashAndSalt("123Dre!").ToDbString()
            };

            mongodb.CreateUser(user);
            Console.WriteLine("User's Id: ", user.Id);

            List<UserModel> users = mongodb.GetAllUsers();

            user.LastName = "Dr.";
            mongodb.UpdateUser(user); // todo: fix db so that the id is updated in the code and not left blank?

            UserModel user1 = mongodb.GetUser(user.Id);

            mongodb.DeleteUser(user1);
        }
    }
}
