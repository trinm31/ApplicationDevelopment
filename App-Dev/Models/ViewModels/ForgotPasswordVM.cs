using System.ComponentModel.DataAnnotations;

namespace App_Dev.Models.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}