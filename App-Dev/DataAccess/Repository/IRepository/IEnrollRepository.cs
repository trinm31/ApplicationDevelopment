using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface IEnrollRepository: IRepositoryAsync<Enroll>
    {
        Task Update(Enroll enroll);
    }
}