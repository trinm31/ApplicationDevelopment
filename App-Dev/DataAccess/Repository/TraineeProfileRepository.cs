using System.Threading.Tasks;
using App_Dev.DataAccess.Data;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Repository
{
    public class TraineeProfileRepository : RepositoryAsync<TraineeProfile>,ITraineeProfileRepository
    {
        private readonly ApplicationDbContext _db;
        public TraineeProfileRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public async Task Update(TraineeProfile traineeProfile)
        {
            var objFromDb = await _db.TraineeProfiles.FirstOrDefaultAsync(s => s.Id == traineeProfile.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = traineeProfile.Name;
                objFromDb.DateOfBirth = traineeProfile.DateOfBirth;
                objFromDb.Education = traineeProfile.Education;
                objFromDb.Age = traineeProfile.Age;
                objFromDb.Department = traineeProfile.Department;
                objFromDb.Location = traineeProfile.Location;
                objFromDb.ToeicScore = traineeProfile.ToeicScore;
                objFromDb.ExperimentDetail = traineeProfile.ExperimentDetail;
                objFromDb.MainProgrammingLanguage = traineeProfile.MainProgrammingLanguage;
            }
        }
    }
}