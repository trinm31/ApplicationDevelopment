using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class AssignToTrainerVM
    {
        public IEnumerable<CourseAssignToTrainer> AssignList { get; set; }
        public IEnumerable<TrainerProfile> TrainerList { get; set; }
    }
}