using DragonflyDataLibrary.Models;

namespace DragonflyMVCApp.Models
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