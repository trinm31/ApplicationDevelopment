using System.Threading.Tasks;
using App_Dev.Models;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository: IRepositoryAsync<ApplicationUser>
    {
        Task Update(ApplicationUser applicationUser);
    }
}