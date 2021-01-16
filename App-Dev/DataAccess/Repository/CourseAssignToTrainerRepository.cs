using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class CourseAssignToTrainerRepository: RepositoryAsync<CourseAssignToTrainer>, ICourseAssignToTrainerRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseAssignToTrainerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(CourseAssignToTrainer courseAssignToTrainer)
        {
            _db.Update(courseAssignToTrainer);
        }
    }
}