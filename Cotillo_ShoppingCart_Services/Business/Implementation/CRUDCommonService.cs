using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Cotillo_ShoppingCart_Services.Domain.Model;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class CRUDCommonService<TEntity> : ICRUDCommonService<TEntity>
        where TEntity : BaseEntity
    {
        #region Fields
        readonly IRepository<TEntity> commonRepository;
        #endregion

        #region Constructor
        public CRUDCommonService(IRepository<TEntity> commonRepository)
        {
            this.commonRepository = commonRepository;
        }
        #endregion

        public virtual async Task<IList<TEntity>> GetAll(int page = 0, int pageSize = int.MaxValue, bool active = true)
        {
            return await commonRepository.GetAllAsync(page, pageSize, active);
        }

        public virtual async Task<TEntity> GetById(int id, bool active = true)
        {
            return await commonRepository.GetByIdAsync(id, active);
        }

        public virtual async Task Save(TEntity entity, bool autoCommit = false)
        {
            if (entity.Id == 0)
            {
                await commonRepository.InsertAsync(entity, autoCommit);
            }
            else
            {
                await commonRepository.UpdateAsync(entity, autoCommit);
            }
        }

        public virtual async Task Delete(TEntity entity, bool softDelete = true, bool autoCommit = false)
        {
            /*
             * NOTE: Uncomment these lines if you want to account for soft delete
            if (softDelete)
            {
                
                entity.Active = false;
                await commonRepository.Update(entity, autoCommit);
            }
            else*/
            await commonRepository.DeleteAsync(entity, autoCommit);
        }

        public virtual async Task Commit()
        {
            await commonRepository.CommitAsync();
        }
    }
}
