using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_Dev.Models.ViewModels
{
    public class OverviewVM
    {
        public IEnumerable<CourseAssignToTrainer> TrainerList { get; set; }
        public IEnumerable<Enroll> TraineeList { get; set; }
    }
}
