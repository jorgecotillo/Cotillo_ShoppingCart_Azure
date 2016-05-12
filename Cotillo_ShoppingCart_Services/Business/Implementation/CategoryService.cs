using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class CategoryService : CRUDCommonService<CategoryEntity>, ICategoryService
    {
        readonly IRepository<CategoryEntity> _categoryRepository;
        public CategoryService(IRepository<CategoryEntity> categoryRepository)
            :base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IList<CategorySummaryDTO>> GetSummaryViewAsync()
        {
            try
            {
                return await (from cat in _categoryRepository.Table
                                         where cat.Active == true
                                         select new CategorySummaryDTO()
                                         {
                                             CategoryName = cat.Name,
                                             ProductCount = cat.Products.Count,
                                             Location = cat.Location
                                         }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
