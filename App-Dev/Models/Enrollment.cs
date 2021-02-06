using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Dev.Models
{
    public class Enrollment
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [Required]
        public string TraineeId { get; set; }
        [ForeignKey("TraineeId")]
        public TraineeProfile TraineeProfile { get; set; }
        public DateTime Time { get; set; }
        public string EnrollStatus { get; set; }
    }
}