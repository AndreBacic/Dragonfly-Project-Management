using System;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class OrganizationViewModel
    {
        public string Name { get; set; }
        /// <summary>
        /// The ids of the users invited and assigned to work on this including the admin.
        /// </summary>
        public Guid CreatorOrEditorId { get; set; }

        public string Description { get; set; }
    }
}
