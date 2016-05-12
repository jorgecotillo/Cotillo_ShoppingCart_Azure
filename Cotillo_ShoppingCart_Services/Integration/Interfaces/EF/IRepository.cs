using Cotillo_ShoppingCart_Services.Domain;
using Cotillo_ShoppingCart_Services.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Integration.Interfaces.EF
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        IList<TEntity> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        Task<IList<TEntity>> GetAllAsync(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        TEntity GetById(int id, bool active = true);
        Task<TEntity> GetByIdAsync(int id, bool active = true);
        void Insert(TEntity entity, bool autoCommit);
        Task InsertAsync(TEntity entity, bool autoCommit);
        void Update(TEntity entity, bool autoCommit);
        Task UpdateAsync(TEntity entity, bool autoCommit);
        void Delete(TEntity entity, bool autoCommit);
        Task DeleteAsync(TEntity entity, bool autoCommit);
        IQueryable<TEntity> Table { get; }
        void Commit();
        Task CommitAsync();
        void AddOrUpdate(Expression<Func<TEntity, object>> expression, TEntity entity);
        void AddOrUpdate(TEntity entity);
    }
}
