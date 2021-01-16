using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface ICourseRepository: IRepositoryAsync<Course>
    {
        Task Update(Course course);
    }
}