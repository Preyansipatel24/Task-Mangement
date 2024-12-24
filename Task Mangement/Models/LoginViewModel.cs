using System.ComponentModel.DataAnnotations;

namespace Task_Mangement.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Company Email is required.")]
        public string CompanyEmail { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
