using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bug_Tracker_Front_End__MVC_plus_Razor.Models
{
    public class CreateUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
