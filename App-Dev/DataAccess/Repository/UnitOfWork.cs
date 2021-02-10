using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Course = new CourseRepository(_db);
            CourseCategory = new CourseCategoryRepository(_db);
            CourseTrainer = new CourseTrainerRepository(_db);
            Enrollment = new EnrollmentRepository(_db);
            TraineeProfile = new TraineeProfileRepository(_db);
            TrainerProfile = new TrainerProfileRepository(_db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICourseRepository Course { get; private set;}
        public ICourseCategoryRepository CourseCategory { get; private set;}
        public ICourseTrainerRepository CourseTrainer { get; private set;}
        public IEnrollmentRepository Enrollment { get; private set;}
        public ITraineeProfileRepository TraineeProfile { get; private set;}
        public ITrainerProfileRepository TrainerProfile { get; private set;}

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}