﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Models
{
    /// <summary>
    /// Defines the data we are gathering from the twitter IDP
    /// </summary>
    public class ExtendedUserInfoModel
    {
        [JsonProperty(PropertyName = "Id")]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public int CustomerId { get; set; }
        /// <summary>
        /// Comma separated version
        /// </summary>
        public string Roles { get; set; }
    }
}
