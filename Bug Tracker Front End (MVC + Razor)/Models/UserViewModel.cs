using Bug_Tracker_Library.Models;
using System;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        /// Unique email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public AssignmentModel WorkingAssignment { get; set; }
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
