using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class CourseTrainerVM
    {
        public IEnumerable<CourseAssignToTrainer> CourseAssignToTrainer { get; set; }
        public IEnumerable<Course> Course { get; set; }
    }
}