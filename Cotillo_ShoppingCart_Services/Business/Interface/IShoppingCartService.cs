using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IShoppingCartService : ICRUDCommonService<ShoppingCartItemEntity>
    {
        void DeleteAllItems(List<ShoppingCartItemEntity> shoppingCartItems);
        Task DeleteAllItemsAsync(List<ShoppingCartItemEntity> shoppingCartItems);
        Task DeleteAllByCustomerIdAsync(int customerId);
        Task DeleteByIdAsync(int shoppingCartId);
        Task<IList<ShoppingCartDTO>> GetAllByCustomerIdAsync(int customerId);
    }
}
