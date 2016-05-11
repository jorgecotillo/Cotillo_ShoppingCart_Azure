using Cotillo_ShoppingCart_Models;
using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
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
                    Image = productEntity.Image,
                    Description = productEntity.Description,
                    ExpiresOn = productEntity.ExpiresOn.ToShortDateString(),
                    Location = productEntity.Location,
                    PriceIncTax = productEntity.PriceIncTax,
                    PriceExcTax = productEntity.PriceExcTax
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categorySummary"></param>
        /// <returns></returns>
        public static CategorySummaryModel ToCategorySummaryModel(this CategorySummaryDTO categorySummary)
        {
            if (categorySummary != null)
            {
                return new CategorySummaryModel()
                {
                    CategoryName = categorySummary.CategoryName,
                    ProductCount = categorySummary.ProductCount
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriesSummary"></param>
        /// <returns></returns>
        public static IList<CategorySummaryModel> ToCategorySummaryModelList(this IList<CategorySummaryDTO> categoriesSummary)
        {
            List<CategorySummaryModel> categorySummaryList = new List<CategorySummaryModel>();
            CategorySummaryModel model = null;
            foreach (var item in categoriesSummary)
            {
                model = item.ToCategorySummaryModel();
                if (model != null)
                    categorySummaryList.Add(model);
            }
            return categorySummaryList;
        }

        public static ExtendedUserInfoModel ToUserInfoModel(this UserEntity userEntity)
        {
            if (userEntity != null)
            {
                return new ExtendedUserInfoModel()
                {
                    UserId = userEntity.Id.ToString(),
                    Name = userEntity.DisplayName
                };
            }
            else
                return null;
        }
    }
}
