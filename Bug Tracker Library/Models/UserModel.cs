using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A User of the Bug Tracker application. 
    /// </summary>
    public class UserModel
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// Tasks assigned to the User.
        /// </summary>
        public virtual List<AssignmentModel> Assignments { get; set; }

        public virtual string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
}
