using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class EnrollmentVM
    {
        public IEnumerable<Enrollment> EnrollList { get; set; }
        public IEnumerable<TraineeProfile> TraineeList { get; set; }
    }
}