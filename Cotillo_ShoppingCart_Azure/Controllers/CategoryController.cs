using Cotillo_ShoppingCart_Services.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Cotillo_ShoppingCart_Azure.Models;

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/category")]
    public class CategoryController : ApiController
    {
        readonly ICategoryService _categoryService;
        readonly IProductService _productService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryService"></param>
        /// <param name="productService"></param>
        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var allCategories = await _categoryService.GetAllAsync();
                return Ok(allCategories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("summary-list")]
        public async Task<IHttpActionResult> GetSummary()
        {
            try
            {
                var summaryDto = await _categoryService.GetSummaryViewAsync();
                if (summaryDto != null)
                    return Ok(summaryDto.ToCategorySummaryModelList());
                else
                    return NotFound();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("{categoryId}/products")]
        public async Task<IHttpActionResult> GetAllProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetAllByCategory(categoryId, active: true, includeImage: false);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
