using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class CourseTrainerViewModel
    {
        public IEnumerable<CourseTrainer> AssignList { get; set; }
        public IEnumerable<TrainerProfile> TrainerList { get; set; }
    }
}