using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Dev.Models
{
    public class CourseAssignToTrainer
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [Required]
        public string TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public TrainerProfile TrainerProfile { get; set; }
        public DateTime Time { get; set; }
    }
}