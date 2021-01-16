using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository
{
    public class ApplicationUserRepository: RepositoryAsync<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}