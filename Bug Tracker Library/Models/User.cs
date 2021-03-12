using System;
using System.Collections.Generic;
using System.Text;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A User of the Bug Tracker application. 
    /// </summary>
    class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Tasks assigned to the User.
        /// </summary>
        public List<Assignment> Assignments { get; set; }
    }
}
