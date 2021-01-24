using System.Linq;
using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class CourseCategoryRepository: Repository<CourseCategory>, ICourseCategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseCategoryRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(CourseCategory courseCategory)
        {
            var objFromDb = _db.CourseCategories.FirstOrDefault(s => s.Id == courseCategory.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = courseCategory.Name;
                objFromDb.Description = courseCategory.Description;
            }
        }
    }
}