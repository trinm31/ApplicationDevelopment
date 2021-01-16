using System;
using System.ComponentModel.DataAnnotations;
using App_Dev.Utility.Enum;

namespace App_Dev.Models
{
    public class TrainerProfile
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required] 
        public string Education { get; set; }
        [Required] 
        public int Age { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public TypeOfTrainer TypeOfTrainer { get; set; }
        [Required]
        public string  WorkingPlace { get; set; }
    }
}