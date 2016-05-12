﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Models
{
    public class ShoppingCartModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double PriceIncTax { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
