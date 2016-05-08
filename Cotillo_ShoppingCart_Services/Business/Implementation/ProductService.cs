using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Cotillo_ShoppingCart_Services.Caching.Extension;
using Cotillo_ShoppingCart_Services.Caching.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class ProductService : CRUDCommonService<ProductEntity>, IProductService
    {
        readonly IRepository<ProductEntity> _productRepository;
        readonly ICacheManager _cacheManager;
        readonly IImageService _imageService;

        public ProductService(
            IRepository<ProductEntity> productRepository, 
            ICacheManager cacheManager,
            IImageService imageService)
            :base(productRepository)
        {
            _productRepository = productRepository;
            _cacheManager = cacheManager;
            _imageService = imageService;
        }

        public override async Task<IList<ProductEntity>> GetAll(int page = 0, int pageSize = int.MaxValue, bool active = true)
        {
            try
            {
                //Get all products
                var allProducts = await _productRepository.Table.ToListAsync();

                foreach (var item in allProducts)
                {
                    //Now let's call the cached image
                    item.Image = GetImage(item.Barcode, item.FileName);
                }

                return allProducts;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] GetImage(string barcode, string fileName)
        {
            string base64String = 
                _cacheManager.Get(barcode, () =>
                {
                    return _imageService.GetImageAsBase64String(fileName);
                }, getAsReferenceType: false, jsonSerialize: false);

            return Convert.FromBase64String(base64String);
        }
    }
}
