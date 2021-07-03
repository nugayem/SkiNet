using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext context)
        {
            this._context = context;
        }

        public IRepository<TEntitty> Repository<TEntitty>() where TEntitty : BaseEntity
        {
            if(_repositories ==null) 
                _repositories= new Hashtable();
            
            var type =  typeof(TEntitty).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoryType=  typeof(Repository<>);
                var repositoryInstance= Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntitty)), _context);

                _repositories.Add(type, repositoryInstance);
            }
            return (IRepository<TEntitty>)_repositories[type];
        }
        public async Task<int> Complete()
        { 
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        
    }
}