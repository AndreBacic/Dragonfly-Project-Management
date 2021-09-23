using Bug_Tracker_Library;
using Bug_Tracker_Library.Models;
using System.Collections.Generic;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class ProjectsListViewModel
    {
        public List<IProjectModel> Projects { get; set; }
        public IUserModel User { get; set; }
        public UserPosition UserPosition { get; set; }
    }
}
