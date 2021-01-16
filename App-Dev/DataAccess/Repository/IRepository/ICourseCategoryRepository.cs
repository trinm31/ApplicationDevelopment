using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface ICourseCategoryRepository: IRepositoryAsync<CourseCategory>
    {
        Task Update(CourseCategory courseCategory);
    }
}