using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class CourseRepository : RepositoryAsync<Course>,ICourseRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public async Task Update(Course course)
        {
            var objFromDb = await _db.Courses.FirstOrDefaultAsync(s => s.Id == course.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = course.Name;
                objFromDb.CategoryId = course.CategoryId;
                objFromDb.Description = course.Description;
            }
        }
    }
}