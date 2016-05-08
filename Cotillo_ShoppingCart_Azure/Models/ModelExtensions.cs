using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Azure.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productEntities"></param>
        /// <returns></returns>
        public static IList<ProductModel> ToProductModelList(this IList<ProductEntity> productEntities)
        {
            List<ProductModel> productList = new List<ProductModel>();
            ProductModel model = null;
            foreach (var item in productEntities)
            {
                model = item.ToProductModel();
                if (model != null)
                    productList.Add(model);
            }
            return productList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public static ProductModel ToProductModel(this ProductEntity productEntity)
        {
            if (productEntity != null)
            {
                return new ProductModel()
                {
                    Id = productEntity.Id,
                    Name = productEntity.Name,
                    CreatedDate = productEntity.CreatedOn,
                    Image = productEntity.Image
                };
            }
            else
            {
                return null;
            }
        }
    }
}
