using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Caching.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cotillo_ShoppingCart_Services.Caching.Extension;
using System.Data.Entity;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class RoleService : CRUDCommonService<RoleEntity>, IRoleService
    {
        readonly IRepository<RoleEntity> _roleRepository;
        readonly ICacheManager _cacheManager;
        public RoleService(IRepository<RoleEntity> roleRepository, ICacheManager cacheManager)
            :base(roleRepository)
        {
            _roleRepository = roleRepository;
            _cacheManager = cacheManager;
        }

        public override IList<RoleEntity> GetAll(int page = 0, int pageSize = int.MaxValue, bool active = true)
        {
            try
            {
                string key = "all-roles";
                return _cacheManager.Get(key, () =>
                {
                    return _roleRepository.GetAll(page, pageSize, active);
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
