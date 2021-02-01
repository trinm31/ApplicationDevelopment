using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class EnrollVM
    {
        public IEnumerable<Enroll> EnrollList { get; set; }
        public IEnumerable<TraineeProfile> TraineeList { get; set; }
    }
}