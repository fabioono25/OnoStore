using System.ComponentModel.DataAnnotations;

namespace OnoStore.Identity.API.Models.UserViewModels
{
    public class UserRegistry
    {
        [Required(ErrorMessage = "Field {0} is mandatory")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is mandatory")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Field {0} is mandatory")]
        [EmailAddress(ErrorMessage = "Field {0} with wrong format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is mandatory")]
        [StringLength(100, ErrorMessage = "Field {0} must be between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords are divergent.")]
        public string PasswordConfirmation { get; set; }
    }
}
