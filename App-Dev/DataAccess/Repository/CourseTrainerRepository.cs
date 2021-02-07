using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class CourseTrainerRepository: RepositoryAsync<CourseTrainer>, ICourseTrainerRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseTrainerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(CourseTrainer courseTrainer)
        {
            _db.Update(courseTrainer);
        }
    }
}