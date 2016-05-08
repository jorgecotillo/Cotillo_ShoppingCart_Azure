using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Message
{
    /// <summary>
    /// This entity is not mapped to any table, therefore is not inheriting from BaseEntity
    /// </summary>
    public class EmailEntity
    {
        public string From { get; set; }
        public string To { get; set; }
        public IList<string> CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
