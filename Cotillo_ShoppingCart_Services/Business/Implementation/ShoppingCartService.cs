﻿using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class ShoppingCartService : CRUDCommonService<ShoppingCartItemEntity>, IShoppingCartService
    {
        readonly IRepository<ShoppingCartItemEntity> _shoppingCartItemRepository;
        public ShoppingCartService(IRepository<ShoppingCartItemEntity> shoppingCartItem)
            :base(shoppingCartItem)
        {
            _shoppingCartItemRepository = shoppingCartItem;
        }

        public void DeleteAllItems(List<ShoppingCartItemEntity> shoppingCartItems)
        {
            try
            {
                if (shoppingCartItems != null && shoppingCartItems.Count > 0)
                {
                    foreach (var item in shoppingCartItems)
                    {
                        //Marking all items to be deleted from the context but not commit the changes to the DB yet
                        _shoppingCartItemRepository.Delete(item, autoCommit: false);
                    }

                    //Making one DB request to delete all items from DbContext
                    _shoppingCartItemRepository.Commit();
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAllItemsAsync(List<ShoppingCartItemEntity> shoppingCartItems)
        {
            try
            {
                if (shoppingCartItems != null && shoppingCartItems.Count > 0)
                {
                    foreach (var item in shoppingCartItems)
                    {
                        //Marking all items to be deleted from the context but not commit the changes to the DB yet
                        await _shoppingCartItemRepository.DeleteAsync(item, autoCommit: false);
                    }

                    //Making one DB request to delete all items from DbContext
                    await _shoppingCartItemRepository.CommitAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteByIdAsync(int shoppingCartId)
        {
            try
            {
                var shoppingCartItem = await _shoppingCartItemRepository.GetByIdAsync(shoppingCartId);
                if(shoppingCartItem != null)
                    await _shoppingCartItemRepository.DeleteAsync(shoppingCartItem, autoCommit: true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAllByCustomerIdAsync(int customerId)
        {
            try
            {
                var shoppingCartItems = await _shoppingCartItemRepository
                    .Table
                    .Where(i => i.CustomerId == customerId)
                    .ToListAsync();

                await DeleteAllItemsAsync(shoppingCartItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<ShoppingCartDTO>> GetAllByCustomerIdAsync(int customerId)
        {
            try
            {
                return await _shoppingCartItemRepository
                    .Table
                    .Where(i => i.CustomerId == customerId)
                    .Select(i => new ShoppingCartDTO()
                    {
                        PriceIncTax = i.PriceIncTax,
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = 1,
                        ShoppingCartId = i.Id
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
