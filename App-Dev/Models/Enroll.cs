using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Dev.Models
{
    public class Enroll
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int CourId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [Required]
        public string TraineeId { get; set; }
        [ForeignKey("TraineeId")]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime Time { get; set; }
    }
}