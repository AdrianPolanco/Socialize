using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Socialize.Presentation.Models.Profile
{
    public class EditProfileViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MinLength(1, ErrorMessage = "The name must be at least 1 character")]
        [MaxLength(50, ErrorMessage = "The name must be cannot have more than 50 characters")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]*$",
    ErrorMessage = "Names can only be alphabetic characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MinLength(1, ErrorMessage = "The lastname must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "The lastname must have cannot be more than 100 characters")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]*$",
            ErrorMessage = "Lastnames can only be alphabetic characters")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "The provided email is not valid")]
        [MaxLength(100, ErrorMessage = "Emails cannot be more than 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
        [MaxLength(35, ErrorMessage = "Username must not be more than 35 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Password != ConfirmPassword) yield return new ValidationResult("Password are not equal", new[] { nameof(Password), nameof(ConfirmPassword) });

            if(!string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(ConfirmPassword))
            {
                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d.*\d)(?=.*[\W_])[a-zA-Z\d\W_]{8,}$";

                bool passwordHasValidFormat = Regex.IsMatch(Password, passwordPattern);

                if (!passwordHasValidFormat) yield return new ValidationResult("The password you are trying to set up is not valid", new[] { nameof(Password) });
            }

            if(Image is not null && !Image.ContentType.StartsWith("image")) yield return new ValidationResult("Only image files are allowed", new[] { nameof(Image) });
            // Definir los patrones de número de teléfono válidos para República Dominicana
            var patterns = new[]
            {
            @"^\+1\s?(\(?829\)?|\(?849\)?|\(?809\)?)\s?\d{3}\s?\d{4}$",  // +1 (829) XXX XXXX, +1 (849) XXX XXXX, +1 (809) XXX XXXX
            @"^\+1\s?(829|849|809)\s?\d{3}\s?\d{4}$",                   // +1 829 XXX XXXX, +1 849 XXX XXXX, +1 809 XXX XXXX
            @"^\+1(829|849|809)\d{7}$",                                 // +1829XXXXXXX, +1849XXXXXXX, +1809XXXXXXX
            @"^1(829|849|809)\d{7}$",                                   // 1829XXXXXXX, 1849XXXXXXX, 1809XXXXXXX
            @"^(829|849|809)\d{7}$"                                     // 829XXXXXXX, 849XXXXXXX, 809XXXXXXX
        };

            bool matches = patterns.Any(pattern => Regex.IsMatch(Phone, pattern));
            if (!matches) yield return new ValidationResult("Phone number is not valid", new[] { nameof(Phone) });
        }
    }
}
