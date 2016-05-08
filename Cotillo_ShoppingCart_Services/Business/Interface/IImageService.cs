using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IImageService
    {
        string GetImageAsBase64String(string fileName);
    }
}
