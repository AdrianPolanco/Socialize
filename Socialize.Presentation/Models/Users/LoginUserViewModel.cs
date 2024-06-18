using System.ComponentModel.DataAnnotations;

namespace Socialize.Presentation.Models.Users
{
    public class LoginUserViewModel
    {

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool TriedToAccwssProtectedRoute { get; set; }
    }
}
