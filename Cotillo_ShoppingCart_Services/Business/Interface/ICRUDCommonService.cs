using Cotillo_ShoppingCart_Services.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface ICRUDCommonService<TEntity>
        where TEntity : BaseEntity
    {
        TEntity GetById(int id, bool active = true);
        IList<TEntity> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        void Save(TEntity entity, bool autoCommit = false);
        void Delete(TEntity entity, bool softDelete = true, bool autoCommit = false);
        void Commit();
        Task<TEntity> GetByIdAsync(int id, bool active = true);
        Task<IList<TEntity>> GetAllAsync(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        Task SaveAsync(TEntity entity, bool autoCommit = false);
        Task DeleteAsync(TEntity entity, bool softDelete = true, bool autoCommit = false);
        Task CommitAsync();
    }
}
