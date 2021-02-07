using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_Dev.Models.ViewModels
{
    public class OverviewViewModel
    {
        public IEnumerable<CourseTrainer> TrainerList { get; set; }
        public IEnumerable<Enrollment> TraineeList { get; set; }
    }
}
