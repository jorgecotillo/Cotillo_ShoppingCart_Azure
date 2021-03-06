﻿using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface ICategoryService : ICRUDCommonService<CategoryEntity>
    {
        Task<IList<CategorySummaryDTO>> GetSummaryViewAsync();
    }
}
