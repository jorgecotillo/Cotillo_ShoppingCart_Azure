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
                var allProductInfo = await _productService.GetAll();

                //Translate Product from Service into Product Model using an extension method
                return Ok(allProductInfo.ToProductModelList());
            }
            catch (Exception ex)
            {
                //log the exception

                return new InternalServerErrorResult(Request);
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}