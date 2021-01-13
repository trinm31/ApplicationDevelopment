
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;

namespace App_Dev.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChangesAsync();
        }
    }
}