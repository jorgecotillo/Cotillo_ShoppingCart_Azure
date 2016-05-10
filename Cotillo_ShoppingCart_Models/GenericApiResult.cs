using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public GenericApiResultEnum Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GenericApiResultEnum
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Success = 1,
        /// <summary>
        /// 
        /// </summary>
        Failure = 2
    }
}
