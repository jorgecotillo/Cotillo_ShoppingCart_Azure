using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Cotillo_ShoppingCart_Services.Caching.Extension;
using Cotillo_ShoppingCart_Services.Caching.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class ProductService : CRUDCommonService<ProductEntity>, IProductService
    {
        readonly IRepository<ProductEntity> _productRepository;
        readonly IRepository<CategoryEntity> _categoryRepository;
        readonly ICacheManager _cacheManager;
        readonly IImageService _imageService;

        public ProductService(
            IRepository<ProductEntity> productRepository, 
            IRepository<CategoryEntity> categoryRepository,
            ICacheManager cacheManager,
            IImageService imageService)
            :base(productRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _cacheManager = cacheManager;
            _imageService = imageService;
        }
        

        public async Task<IList<ProductEntity>> GetAllProductsAsync(int page = 0, int pageSize = int.MaxValue, bool active = true, bool includeImage = false)
        {
            try
            {
                if(includeImage)
                {
                    //Get all products
                    var allProducts = await _productRepository
                        .Table
                        .ToListAsync();

                    foreach (var item in allProducts)
                    {
                        //Now let's call the image and retrieve it from the cache or cache it if it doesn't exists
                        item.Image = GetImage(item.Barcode, item.FileName);
                    }

                    return allProducts;
                }
                else
                {
                    //Get all products
                    return await _productRepository.Table.ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private byte[] GetImage(string barcode, string fileName)
        {
            //Get or Set cached item
            string base64String = 
                _cacheManager.Get(barcode, () =>
                {
                    return _imageService.GetImageAsBase64String(fileName);
                }, getAsReferenceType: false, jsonSerialize: false);

            return Convert.FromBase64String(base64String);
        }

        public async Task<IList<ProductEntity>> GetAllByCategory(int categoryId, bool active = true, bool includeImage = false)
        {
            try
            {
                IList<ProductEntity> allProducts = await (from prod in _productRepository.Table
                                                         join cat in _categoryRepository.Table on prod.CategoryId equals cat.Id
                                                         where prod.Active == true && cat.Active == true
                                                         select prod).ToListAsync();
                if (includeImage)
                {
                    foreach (var item in allProducts)
                    {
                        //Now let's call the image and retrieve it from the cache or cache it if it doesn't exists
                        item.Image = GetImage(item.Barcode, item.FileName);
                    }

                    return allProducts;
                }
                else
                {
                    return allProducts;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductEntity> GetByBarcodeAsync(string barcode)
        {
            try
            {
                return await _productRepository
                    .Table
                    .Where(i => i.Barcode == barcode)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
