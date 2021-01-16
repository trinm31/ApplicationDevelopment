using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App_Dev.Utility.Enum;

namespace App_Dev.Models
{
    public class TraineeProfile: ApplicationUser
    {
        [Required] 
        public string MainProgrammingLanguage { get; set; }
        [Required] 
        public int ToeicScore { get; set; }
        [Required]
        public string  ExperimentDetail { get; set; }
        [Required]
        public Department  Department { get; set; }
        [Required]
        public string Location { get; set; }
    }
}