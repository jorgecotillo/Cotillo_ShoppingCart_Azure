using Cotillo_ShoppingCart_Azure.Models;
using Cotillo_ShoppingCart_Services.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsController : ApiController
    {
        readonly IProductService _productService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(IProductService productService)
        {
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
                var allProductInfo = await _productService.GetAllAsync();

                //Translate Product from Service into Product Model using an extension method
                return Ok(allProductInfo.ToProductModelList());
            }
            catch (Exception ex)
            {
                //log the exception

                return new InternalServerErrorResult(Request);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get(int id)
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetByCategory(int categoryId)
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetAllProductCategories()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            return Ok();
        }
        
        public async Task<IHttpActionResult> Post()
        {
            return Ok();
        }
    }
}