using System;

namespace App_Dev.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}