using DragonflyDataLibrary.Models;
using System;
using System.Collections.Generic;

namespace DragonflyMVCApp.Models
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
        public Dictionary<Guid, UserModel> Workers { get; set; }
    }
}
