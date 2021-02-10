using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class EnrollmentRepository: RepositoryAsync<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _db;
        public EnrollmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Enrollment enrollment)
        {
            _db.Update(enrollment);
        }
    }
}