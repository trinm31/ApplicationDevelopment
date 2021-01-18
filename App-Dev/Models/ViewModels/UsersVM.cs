using App_Dev.Utility.Enum;

namespace App_Dev.Models.ViewModels
{
    public class UsersVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public TraineeProfile TraineeProfile { get; set; }
        public TrainerProfile TrainerProfile { get; set; }
    }
}