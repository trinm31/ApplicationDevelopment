using System;
using System.Threading.Tasks;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ICourseRepository Course { get; }
        ICourseCategoryRepository CourseCategory { get; }
        ICourseTrainerRepository CourseTrainer { get;}
        IEnrollmentRepository Enrollment { get; }
        ITraineeProfileRepository TraineeProfile { get; }
        ITrainerProfileRepository TrainerProfile { get; }
        void Save();
    }
}