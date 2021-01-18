using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace App_Dev.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required] 
        public string Education { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}