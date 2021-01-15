using System.ComponentModel.DataAnnotations;
using App_Dev.Utility.Enum;

namespace App_Dev.Models
{
    public class TraineeProfile
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string MainProgrammingLanguage { get; set; }
        [Required] 
        public int ToeicScore { get; set; }
        [Required]
        public string  ExperimentDetail { get; set; }
        [Required]
        public Department  Department { get; set; }
    }
}