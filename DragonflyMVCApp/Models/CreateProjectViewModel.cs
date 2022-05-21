using System;
using System.ComponentModel.DataAnnotations;

namespace DragonflyMVCApp.Models
{
    public class CreateProjectViewModel
    {
        [Required]
        [MaxLength(60)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// Not required, but if left blank will default to the max value, which is 12/31 9999 11:59:59 PM
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; } = DateTime.MaxValue;
        /// <summary>
        /// If left blank will default to $0.00
        /// </summary>
        [DataType(DataType.Currency)]
        public decimal Budget { get; set; } = 0m;
    }
}
