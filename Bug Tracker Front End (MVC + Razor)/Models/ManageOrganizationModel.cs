using Bug_Tracker_Library.Models;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class ManageOrganizationModel
    {
        /// <summary>
        /// Empty if this is the first display, 
        /// True if an update was successfully performed, 
        /// or False if the update couldn't be done.
        /// </summary>
        public string DidUpdateOrg { get; set; }
        public OrganizationModel Organization { get; set; }
        public UserModel LoggedInUser { get; set; }
    }
}
