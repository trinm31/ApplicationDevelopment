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
        public int TrainerProfileId { get; set; }
        [ForeignKey("TrainerProfileId")]
        public TrainerProfile TrainerProfile { get; set; }
        [Required]
        public int TraineeProfileId { get; set; }
        [ForeignKey("TraineeProfileId")]
        public TraineeProfile TraineeProfile { get; set; }
        [NotMapped] 
        public string Role { get; set; }
    }
}