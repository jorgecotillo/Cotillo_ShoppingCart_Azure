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
    [RoutePrefix("api/v1/products")]
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
                var allProductInfo = await _productService.GetAllProductsAsync(includeImage: true);

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
        /// <returns></returns>
        [Route("no-image")]
        public async Task<IHttpActionResult> GetProductsWithoutImages()
        {
            try
            {
                var allProductInfo = await _productService.GetAllProductsAsync(includeImage: false);

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
        /// <param name="barcode"></param>
        /// <returns></returns>
        [Route("barcode/{barcode}")]
        public async Task<IHttpActionResult> GetByBarcode(string barcode)
        {
            try
            {
                var product = await _productService.GetByBarcodeAsync(barcode);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResult(Request);
            }
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