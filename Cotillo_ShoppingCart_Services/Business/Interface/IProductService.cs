using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IProductService : ICRUDCommonService<ProductEntity>
    {
        Task<IList<ProductEntity>> GetAllProductsAsync(int page = 0, int pageSize = int.MaxValue, bool active = true, bool includeImage = false);
        Task<IList<ProductEntity>> GetAllByCategory(int categoryId, bool active = true, bool includeImage = false);
    }
}
