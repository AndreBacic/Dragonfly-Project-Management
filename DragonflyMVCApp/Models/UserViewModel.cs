using System;

namespace DragonflyMVCApp.Models
{
    public class UserViewModel
    {
        // TODO: Make this view model different from the data model or just use the data model directly
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        /// Unique email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Name => FirstName + " " + LastName;
    }
}
