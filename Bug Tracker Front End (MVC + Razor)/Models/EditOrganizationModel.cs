using System;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class EditOrganizationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}