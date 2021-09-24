using Bug_Tracker_Library.DataAccess.MongoDB;
using Bug_Tracker_Library.Models;
using Bug_Tracker_Library.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
// todo: put this back in it's original folder and figure out how to get the c# compiler to recoginize this program's Main.
namespace Bug_Tracker.Tests.Library.DataAccess
{
    public class MongoDBTests
    {
        [Fact]
        public void MongoDbDataAccess_ShouldWork()
        {
            var mongodb = new MongoDBDataAccessor("BugTracker");

            //CRUDUserModel(mongodb);
        }

        private void CRUDUserModel(MongoDBDataAccessor mongodb)
        {
            var user = new UserModel()
            {
                EmailAddress = "dre@dre.net",
                FirstName = "Dre",
                LastName = "AlsoDre",
                PhoneNumber = "123-456-7890",
                PasswordHash = HashAndSalter.HashAndSalt("123Dre!").ToDbString()
            };

            mongodb.CreateUser(user);
            Console.WriteLine("User's Id: ", user.Id);

            var users = mongodb.GetAllUsers();

            user.LastName = "Dr.";
            mongodb.UpdateUser(user); // todo: fix db so that the id is updated in the code and not left blank?

            var user1 = mongodb.GetUser(user.Id);

            mongodb.DeleteUser(user1);
        }
    }
}
