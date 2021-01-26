using System.Collections.Generic;

namespace App_Dev.Models.ViewModels
{
    public class EnrollVM
    {
        public Course Course { get; set; }
        public IEnumerable<TraineeProfile> TraineeList { get; set; }
    }
}