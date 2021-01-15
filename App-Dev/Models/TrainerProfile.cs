using System.ComponentModel.DataAnnotations;
using App_Dev.Utility.Enum;

namespace App_Dev.Models
{
    public class TrainerProfile
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public TypeOfTrainer TypeOfTrainer { get; set; }
        [Required]
        public string  WorkingPlace { get; set; }
    }
}