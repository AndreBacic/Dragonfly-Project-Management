using DragonflyDataLibrary.Models;
using System;
using System.Collections.Generic;

namespace DragonflyMVCApp.Models
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
