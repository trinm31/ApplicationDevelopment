using System;
using System.ComponentModel.DataAnnotations;
using App_Dev.Utility.Enum;

namespace App_Dev.Models
{
    public class TraineeProfile
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required] 
        public string Education { get; set; }
        [Required] 
        public int Age { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
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