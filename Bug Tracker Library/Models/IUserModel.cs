using System;
using System.Collections.Generic;

namespace Bug_Tracker_Library.Models
{
    /// <summary>
    /// A User of the Bug Tracker application. 
    /// <br/>
    /// NOTE: User.EmailAddress must be unique.
    /// </summary>
    public interface IUserModel
    {
         Guid Id { get; set; }
         string FirstName { get; set; }
         string LastName { get; set; }
         string EmailAddress { get; set; }
         string PhoneNumber { get; set; }
         string PasswordHash { get; set; }

        /// <summary>
        /// Tasks assigned to the User.
        /// </summary>
         List<IAssignmentModel> Assignments { get; set; }

         string Name { get; }
    }
}
