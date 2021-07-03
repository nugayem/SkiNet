using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntitty> Repository<TEntitty>()  where TEntitty : BaseEntity;
        Task<int> Complete();
    }
}