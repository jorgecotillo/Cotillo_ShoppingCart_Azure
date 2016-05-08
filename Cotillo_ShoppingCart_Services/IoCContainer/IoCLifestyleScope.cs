using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.IoCContainer
{
    public enum IoCLifestyleScope
    {
        None = 0,
        WebRequest = 1,
        WebApi = 2
    }
}
