using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface ITrainerProfileRepository: IRepositoryAsync<TrainerProfile>
    {
        Task Update(TrainerProfile trainerProfile);
    }
}