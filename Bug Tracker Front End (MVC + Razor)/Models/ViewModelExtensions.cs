using Bug_Tracker_Library.Models;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public static class ViewModelExtensions { 
        public static EditUserViewModel DbUserToEditView(this UserModel user)
        {
            return new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };
        }
    }
}