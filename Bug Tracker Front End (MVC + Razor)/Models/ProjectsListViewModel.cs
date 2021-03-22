using Bug_Tracker_Library.Models;
using Bug_Tracker_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class ProjectsListViewModel
    {
        public List<ProjectModel> Projects { get; set; }
        public UserModel User { get; set; }
        public UserPosition UserPosition { get; set; }
    }
}
