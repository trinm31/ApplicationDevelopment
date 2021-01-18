using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App_Dev.Utility.Enum;

namespace App_Dev.Models
{
    public class TrainerProfile : ApplicationUser
    {
        [Required]
        public TypeOfTrainer TypeOfTrainer { get; set; }
        [Required]
        public string  WorkingPlace { get; set; }
    }
}