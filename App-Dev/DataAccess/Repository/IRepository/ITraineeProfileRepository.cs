using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface ITraineeProfileRepository: IRepositoryAsync<TraineeProfile>
    {
        Task Update(TraineeProfile traineeProfile);
    }
}