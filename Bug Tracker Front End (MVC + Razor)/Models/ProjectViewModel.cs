using Bug_Tracker_Library.Models;
using System;
using System.Collections.Generic;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class ProjectViewModel
    {
        public ProjectModel Project { get; set; }
        /// <summary>
        /// The users assigned to work on this.Project
        /// </summary>
        public Dictionary<Guid, UserModel> Workers { get; set; } = new();
    }
}
