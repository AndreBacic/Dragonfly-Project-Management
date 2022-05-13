namespace DragonflyMVCApp.Models
{
    public class CreateUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
