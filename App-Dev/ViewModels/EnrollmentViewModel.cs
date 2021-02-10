using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public IEnumerable<Enrollment> EnrollList { get; set; }
        public IEnumerable<TraineeProfile> TraineeList { get; set; }
    }
}