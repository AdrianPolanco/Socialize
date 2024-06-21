using System.ComponentModel.DataAnnotations;

namespace Socialize.Presentation.Models.Users
{
    public class ResetUserPasswordViewModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
        [MaxLength(35, ErrorMessage = "Username must not be more than 35 characters")]
        public string Username { get; set; }
    }
}
