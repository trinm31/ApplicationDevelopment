using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface IEnrollmentRepository: IRepositoryAsync<Enrollment>
    {
        Task Update(Enrollment enrollment);
    }
}