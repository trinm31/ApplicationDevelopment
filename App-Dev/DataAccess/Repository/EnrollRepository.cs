using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class EnrollRepository: RepositoryAsync<Enroll>, IEnrollRepository
    {
        private readonly ApplicationDbContext _db;
        public EnrollRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Enroll enroll)
        {
            _db.Update(enroll);
        }
    }
}