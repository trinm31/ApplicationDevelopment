using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface ICourseTrainerRepository: IRepositoryAsync<CourseTrainer>
    {
        Task Update(CourseTrainer courseTrainer);
    }
}