using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface ICourseAssignToTrainerRepository: IRepositoryAsync<CourseAssignToTrainer>
    {
        Task Update(CourseAssignToTrainer courseAssignToTrainer);
    }
}