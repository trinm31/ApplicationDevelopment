using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class TrainerProfileRepository : RepositoryAsync<TrainerProfile>,ITrainerProfileRepository
    {
        private readonly ApplicationDbContext _db;
        public TrainerProfileRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public async Task Update(TrainerProfile trainerProfile)
        {
            var objFromDb = await _db.TrainerProfiles.FirstOrDefaultAsync(s => s.Id == trainerProfile.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = trainerProfile.Name;
                objFromDb.PhoneNumber = trainerProfile.PhoneNumber;
                objFromDb.WorkingPlace = trainerProfile.WorkingPlace;
                objFromDb.DateOfBirth = trainerProfile.DateOfBirth;
                objFromDb.TypeOfTrainer = trainerProfile.TypeOfTrainer;
                objFromDb.Email = trainerProfile.Email;
                objFromDb.Education = trainerProfile.Education;
            }
        }
    }
}